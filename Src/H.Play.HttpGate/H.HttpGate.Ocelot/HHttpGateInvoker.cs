using H.HttpGate.Contracts.Public;
using H.HttpGate.Contracts.Public.DataContracts;
using H.Necessaire;
using Ocelot.Middleware;
using Ocelot.Request.Middleware;

namespace H.HttpGate.Ocelot
{
    internal class HHttpGateInvoker
    {
        #region Construct
        ImAnHsHttpGateActionRegistry gateActionRegistry;
        ImALogger log;
        public HHttpGateInvoker(ImAnHsHttpGateActionRegistry gateActionRegistry, Func<Type, ImALogger> logProvider)
        {
            this.gateActionRegistry = gateActionRegistry;
            this.log = logProvider(typeof(HHttpGateInvoker));
        }
        #endregion

        public async Task<OperationResult> Run(HttpContext httpContext)
        {
            if (gateActionRegistry is null)
                return OperationResult.Win();

            IEnumerable<ImAnHsHttpGateAction> gateActionsStream = await gateActionRegistry.StreamAllKnownActions();
            if (gateActionsStream is null)
                return OperationResult.Win();

            HSafe.Run(() => { if (httpContext.Request.Body.CanSeek) httpContext.Request.Body.Position = 0; });
            HSafe.Run(() => { if (httpContext.Response.Body.CanSeek) httpContext.Response.Body.Position = 0; });

            if (!(await HSafe.Run(async () => await Build(httpContext.Items.DownstreamRequest(), httpContext.Items.DownstreamResponse())).LogError(log, "Build HHttpGateResponse for Hs HttpGate Actions")).Ref(out var dataBuildRes, out var data))
                return dataBuildRes;

            foreach (ImAnHsHttpGateAction gateAction in gateActionsStream)
            {
                string actionTag = $"Run HttpGate Action: {gateAction.GetType().Name}";
                await HSafe.Run(async () => await gateAction.Run(data).LogError(log, actionTag)).LogError(log, actionTag);
            }

            return OperationResult.Win();
        }

        async Task<HHttpGateResponse> Build(DownstreamRequest ocelotRequest, DownstreamResponse ocelotResponse)
        {
            string ocelotRequestContent = null;
            string ocelotResponseContent = null;

            if (ocelotRequest?.HasContent == true)
            {
                ocelotRequestContent = await ocelotRequest.Request.Content.ReadAsStringAsync();
            }
            ocelotResponseContent = ocelotResponse is null ? null : await ocelotResponse.Content.ReadAsStringAsync();

            return new HHttpGateResponse
            {
                Request = ocelotRequest.MorphIfNotNull(ocelotRequest => new HHttpGateRequest
                {
                    URL = ocelotRequest.ToUri(),
                    Headers = ocelotRequest.Headers?.ToDictionary(h => h.Key, h => h.Value?.ToNonEmptyArray()),
                    HttpMethod = ocelotRequest.Method,
                    HttpVersion = ocelotRequest.Request?.Version?.ToString(),
                    Content = ocelotRequestContent,
                }),
                Headers = ocelotResponse?.Headers?.ToDictionary(h => h.Key, h => h.Values?.ToNonEmptyArray()),
                HttpStatusCode = (int?)(ocelotResponse?.StatusCode) ?? -1,
                HttpStatusLabel = ocelotResponse?.StatusCode.ToString(),
                HttpStatusReason = ocelotResponse?.ReasonPhrase,
                Content = ocelotResponseContent,
            };
        }
    }
}
