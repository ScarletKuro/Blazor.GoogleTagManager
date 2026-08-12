import { afterEach, beforeEach, describe, expect, test } from "bun:test";
import * as gtmModule from "./GoogleTagManager";

type DataLayerEvent = Record<string, unknown>;

type TestWindow = {
    dataLayer?: DataLayerEvent[];
    isGTM?: boolean;
    [key: string]: unknown;
};

type TestScriptElement = {
    tagName: string;
    async?: boolean;
    src?: string;
    attributes: Record<string, string>;
    setAttribute(name: string, value: string): void;
};

type TestDocument = {
    createElement(tagName: string): TestScriptElement;
    getElementsByTagName(tagName: string): Array<{
        appendChild(node: TestScriptElement): void;
    }>;
};

const runtime = globalThis as typeof globalThis & {
    window?: TestWindow;
    document?: TestDocument;
};

let previousWindow: TestWindow | undefined;
let previousDocument: TestDocument | undefined;
let previousConsoleLog: typeof console.log;

beforeEach(() => {
    previousWindow = runtime.window;
    previousDocument = runtime.document;
    previousConsoleLog = console.log;
});

afterEach(() => {
    runtime.window = previousWindow;
    runtime.document = previousDocument;
    console.log = previousConsoleLog;
});

function createScriptElement(tagName: string): TestScriptElement
{
    return {
        tagName,
        attributes: {},
        setAttribute(name, value) {
            this.attributes[name] = value;
        }
    };
}

function setupDocument(onAppendChild?: (node: TestScriptElement) => void): void
{
    runtime.document = {
        createElement(tagName) {
            return createScriptElement(tagName);
        },
        getElementsByTagName(tagName) {
            expect(tagName).toBe("head");

            return [{
                appendChild(node) {
                    onAppendChild?.(node);
                }
            }];
        }
    };
}

function createConsoleSpy(): string[]
{
    const logs: string[] = [];
    console.log = ((message: string) => {
        logs.push(message);
    }) as typeof console.log;

    return logs;
}

describe("buildScriptUrl", () => {
    test("uses the default GTM host format when no query parameters are provided", () => {
        const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX");

        expect(scriptUrl).toBe("https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX");
    });

    test("appends GTM environment query parameters", () => {
        const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX", {
            gtm_auth: "example-auth-token",
            gtm_preview: "env-3"
        });

        expect(scriptUrl).toBe("https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&gtm_auth=example-auth-token&gtm_preview=env-3");
    });

    test("URL-encodes query parameter keys and values", () => {
        const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX", {
            "gtm preview": "env/3 value",
            "gtm+auth": "token=value"
        });

        expect(scriptUrl).toBe("https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&gtm%20preview=env%2F3%20value&gtm%2Bauth=token%3Dvalue");
    });

    test("ignores empty query parameter entries", () => {
        const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX", {
            gtm_auth: "",
            gtm_preview: null,
            gtm_cookies_win: undefined,
            env_name: "staging"
        });

        expect(scriptUrl).toBe("https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&env_name=staging");
    });
});

describe("initialize", () => {
    test("applies attributes to the script element without moving them into the URL", () => {
        let appendedScript: TestScriptElement | undefined;

        runtime.window = {};
        setupDocument((node) => {
            appendedScript = node;
        });

        gtmModule.initialize(
            "https://www.googletagmanager.com",
            "GTM-XXXXXXX",
            { "data-consent-category": "google" },
            false,
            { gtm_auth: "auth-token" });

        expect(appendedScript?.src).toBe("https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&gtm_auth=auth-token");
        expect(appendedScript?.attributes).toEqual({ "data-consent-category": "google" });
    });

    test("accepts an options object payload and bootstraps the dataLayer", () => {
        let appendedScript: TestScriptElement | undefined;
        runtime.window = {};
        setupDocument((node) => {
            appendedScript = node;
        });

        gtmModule.initialize({
            url: "https://www.googletagmanager.com",
            gtmId: "GTM-YYYYYYY",
            attributes: { nonce: "abc123" },
            debugToConsole: false,
            queryParameters: { gtm_preview: "env-9" }
        });

        expect(appendedScript?.src).toBe("https://www.googletagmanager.com/gtm.js?id=GTM-YYYYYYY&gtm_preview=env-9");
        expect(appendedScript?.attributes).toEqual({ nonce: "abc123" });
        expect(runtime.window?.isGTM).toBe(true);
        expect(runtime.window?.dataLayer).toHaveLength(2);
        expect(runtime.window?.dataLayer?.[0]?.event).toBe("gtm.js");
        expect(typeof runtime.window?.dataLayer?.[0]?.["gtm.start"]).toBe("number");
        expect(runtime.window?.dataLayer?.[1]).toEqual({ event: "pageview" });
    });

    test("writes debug output when enabled", () => {
        const logs = createConsoleSpy();

        runtime.window = {};
        setupDocument();

        gtmModule.initialize("https://www.googletagmanager.com", "GTM-DEBUG", {}, true, {});

        expect(logs).toEqual(["[GTM]: Configured with URL = https://www.googletagmanager.com, and GtmId = GTM-DEBUG"]);
    });

});

describe("push helpers", () => {
    test("push writes raw payloads to window.dataLayer", () => {
        runtime.window = { dataLayer: [] };

        gtmModule.push({ event: "custom", value: 42 });

        expect(runtime.window.dataLayer).toEqual([{ event: "custom", value: 42 }]);
    });

    test("pushEvent injects the event name and logs only when enabled", () => {
        const logs = createConsoleSpy();
        runtime.window = { dataLayer: [] };

        gtmModule.pushEvent("button_click_sample_event", { category: "cta" }, true);

        expect(runtime.window.dataLayer).toEqual([{
            category: "cta",
            event: "button_click_sample_event"
        }]);
        expect(logs).toEqual(['[GTM]:{"category":"cta","event":"button_click_sample_event"}']);
    });

    test("pushPageViewEvent injects both the URL variable and event name", () => {
        runtime.window = { dataLayer: [] };

        gtmModule.pushPageViewEvent("virtualPageView", "pageUrl", "/counter", { isNavigationIntercepted: true });

        expect(runtime.window.dataLayer).toEqual([{
            isNavigationIntercepted: true,
            pageUrl: "/counter",
            event: "virtualPageView"
        }]);
    });

});
