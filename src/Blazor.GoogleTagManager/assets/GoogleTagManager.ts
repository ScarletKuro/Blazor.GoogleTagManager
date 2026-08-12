type QueryParameterValue = string | null | undefined;
type QueryParameters = Record<string, QueryParameterValue>;
type ScriptAttributes = Record<string, string>;
type EventData = Record<string, unknown>;

type InitializationOptionsInput = {
    url?: string;
    Url?: string;
    gtmId?: string;
    GtmId?: string;
    attributes?: ScriptAttributes;
    Attributes?: ScriptAttributes;
    debugToConsole?: boolean;
    DebugToConsole?: boolean;
    queryParameters?: QueryParameters;
    QueryParameters?: QueryParameters;
}

type NormalizedInitializationOptions = {
    url: string | undefined;
    GTMID: string | undefined;
    attributes: ScriptAttributes;
    debugToConsole: boolean;
    queryParameters: QueryParameters;
};

declare global {
    interface Window {
        dataLayer?: EventData[];
        isGTM?: boolean;
    }
}

export function buildScriptUrl(
    url: string,
    GTMID: string,
    queryParameters: QueryParameters = {},
    dataLayerName = "dataLayer")
{
    const additionalParameters = Object.entries(queryParameters ?? {})
        .filter(([key, value]) => key && value)
        .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(value)}`)
        .join("&");
    const dataLayerParameter = dataLayerName !== "dataLayer" ? `&l=${dataLayerName}` : "";
    const queryString = additionalParameters ? `&${additionalParameters}` : "";

    return `${url}/gtm.js?id=${GTMID}${dataLayerParameter}${queryString}`;
}

function normalizeInitializationOptions(
    urlOrOptions: string | InitializationOptionsInput,
    GTMID?: string,
    attributes?: ScriptAttributes,
    debugToConsole = false,
    queryParameters: QueryParameters = {}): NormalizedInitializationOptions
{
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

export function initialize(options: InitializationOptionsInput): void;
export function initialize(
    url: string,
    GTMID: string,
    attributes?: ScriptAttributes,
    debugToConsole?: boolean,
    queryParameters?: QueryParameters): void;
export function initialize(
    urlOrOptions: string | InitializationOptionsInput,
    GTMID?: string,
    attributes?: ScriptAttributes,
    debugToConsole = false,
    queryParameters: QueryParameters = {})
{
    const initializationOptions = normalizeInitializationOptions(
        urlOrOptions,
        GTMID,
        attributes,
        debugToConsole,
        queryParameters);

    window.dataLayer = window.dataLayer || [];
    window.dataLayer.push({
        "gtm.start": Date.now(),
        event: "gtm.js"
    });

    const head = document.getElementsByTagName("head")[0];
    const script = document.createElement("script");
    script.async = true;
    script.src = buildScriptUrl(
        initializationOptions.url as string,
        initializationOptions.GTMID as string,
        initializationOptions.queryParameters);

    for (const [key, value] of Object.entries(initializationOptions.attributes)) {
        script.setAttribute(key, value);
    }

    head.appendChild(script, head);
    window.dataLayer.push({ event: "pageview" });
    window.isGTM = true;

    if (initializationOptions.debugToConsole) {
        console.log(`[GTM]: Configured with URL = ${initializationOptions.url}, and GtmId = ${initializationOptions.GTMID}`);
    }
}

export function push(data: EventData, debugToConsole = false)
{
    window.dataLayer.push(data);

    if (debugToConsole) {
        console.log(`[GTM]:${JSON.stringify(data)}`);
    }
}

export function pushEvent(eventName: string, eventData: EventData | null, debugToConsole = false)
{
    if (eventData === null) {
        eventData = {};
    }

    eventData.event = eventName;
    push(eventData, debugToConsole);
}

export function pushPageViewEvent(
    eventName: string,
    urlVariableName: string,
    url: string,
    eventData: EventData | null,
    debugToConsole = false)
{
    if (eventData === null) {
        eventData = {};
    }

    eventData[urlVariableName] = url;
    eventData.event = eventName;
    push(eventData, debugToConsole);
}
