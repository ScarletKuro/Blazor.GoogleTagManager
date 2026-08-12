// assets/GoogleTagManager.ts
function buildScriptUrl(url, GTMID, queryParameters = {}, dataLayerName = "dataLayer") {
  const additionalParameters = Object.entries(queryParameters ?? {}).filter(([key, value]) => key && value).map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(value)}`).join("&");
  const dataLayerParameter = dataLayerName !== "dataLayer" ? `&l=${dataLayerName}` : "";
  const queryString = additionalParameters ? `&${additionalParameters}` : "";
  return `${url}/gtm.js?id=${GTMID}${dataLayerParameter}${queryString}`;
}
function normalizeInitializationOptions(urlOrOptions, GTMID, attributes, debugToConsole = false, queryParameters = {}) {
  if (typeof urlOrOptions === "object" && urlOrOptions !== null) {
    return {
      url: urlOrOptions.url ?? urlOrOptions.Url,
      GTMID: urlOrOptions.gtmId ?? urlOrOptions.GtmId,
      attributes: urlOrOptions.attributes ?? urlOrOptions.Attributes ?? {},
      debugToConsole: urlOrOptions.debugToConsole ?? urlOrOptions.DebugToConsole ?? false,
      queryParameters: urlOrOptions.queryParameters ?? urlOrOptions.QueryParameters ?? {}
    };
  }
  return {
    url: urlOrOptions,
    GTMID,
    attributes: attributes ?? {},
    debugToConsole,
    queryParameters: queryParameters ?? {}
  };
}
function initialize(urlOrOptions, GTMID, attributes, debugToConsole = false, queryParameters = {}) {
  const initializationOptions = normalizeInitializationOptions(urlOrOptions, GTMID, attributes, debugToConsole, queryParameters);
  (function(w, d, s, l, u, i, m, k, q) {
    w[l] = w[l] || [];
    w[l].push({
      "gtm.start": new Date().getTime(),
      event: "gtm.js"
    });
    const head = d.getElementsByTagName("head")[0];
    const script = d.createElement(s);
    script.async = true;
    script.src = buildScriptUrl(u, i, q, l);
    for (const [key, value] of Object.entries(m)) {
      script.setAttribute(key, value);
    }
    head.appendChild(script, head);
    window.dataLayer.push({ event: "pageview" });
    window.isGTM = true;
    if (k) {
      console.log(`[GTM]: Configured with URL = ${u}, and GtmId = ${i}`);
    }
  })(window, document, "script", "dataLayer", initializationOptions.url, initializationOptions.GTMID, initializationOptions.attributes, initializationOptions.debugToConsole, initializationOptions.queryParameters);
}
function push(data, debugToConsole = false) {
  window.dataLayer.push(data);
  if (debugToConsole) {
    console.log(`[GTM]:${JSON.stringify(data)}`);
  }
}
function pushEvent(eventName, eventData, debugToConsole = false) {
  if (eventData === null) {
    eventData = {};
  }
  eventData.event = eventName;
  push(eventData, debugToConsole);
}
function pushPageViewEvent(eventName, urlVariableName, url, eventData, debugToConsole = false) {
  if (eventData === null) {
    eventData = {};
  }
  eventData[urlVariableName] = url;
  eventData.event = eventName;
  push(eventData, debugToConsole);
}
export {
  pushPageViewEvent,
  pushEvent,
  push,
  initialize,
  buildScriptUrl
};
