import assert from "node:assert/strict";
import { readFile } from "node:fs/promises";
import path from "node:path";
import test from "node:test";
import { fileURLToPath } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const modulePath = path.resolve(__dirname, "../src/Blazor.GoogleTagManager/wwwroot/GoogleTagManager.js");
const moduleSource = await readFile(modulePath, "utf8");
const gtmModule = await import(`data:text/javascript;charset=utf-8,${encodeURIComponent(moduleSource)}`);

test("buildScriptUrl uses the default GTM host format when no query parameters are provided", () => {
    const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX");

    assert.equal(scriptUrl, "https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX");
});

test("buildScriptUrl appends GTM environment query parameters", () => {
    const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX", {
        gtm_auth: "example-auth-token",
        gtm_preview: "env-3"
    });

    assert.equal(scriptUrl, "https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&gtm_auth=example-auth-token&gtm_preview=env-3");
});

test("buildScriptUrl URL-encodes query parameter keys and values", () => {
    const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX", {
        "gtm preview": "env/3 value",
        "gtm+auth": "token=value"
    });

    assert.equal(scriptUrl, "https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&gtm%20preview=env%2F3%20value&gtm%2Bauth=token%3Dvalue");
});

test("buildScriptUrl ignores empty query parameter entries", () => {
    const scriptUrl = gtmModule.buildScriptUrl("https://www.googletagmanager.com", "GTM-XXXXXXX", {
        gtm_auth: "",
        gtm_preview: null,
        gtm_cookies_win: undefined,
        env_name: "staging"
    });

    assert.equal(scriptUrl, "https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&env_name=staging");
});

test("initialize applies attributes to the script element without moving them into the URL", () => {
    let appendedScript = null;
    const previousWindow = globalThis.window;
    const previousDocument = globalThis.document;

    globalThis.window = {};
    globalThis.document = {
        createElement(tagName) {
            return {
                tagName,
                attributes: {},
                setAttribute(name, value) {
                    this.attributes[name] = value;
                }
            };
        },
        getElementsByTagName(tagName) {
            assert.equal(tagName, "head");
            return [{
                appendChild(node) {
                    appendedScript = node;
                }
            }];
        }
    };

    try {
        gtmModule.initialize(
            "https://www.googletagmanager.com",
            "GTM-XXXXXXX",
            { "data-consent-category": "google" },
            false,
            { gtm_auth: "auth-token" });
    } finally {
        globalThis.window = previousWindow;
        globalThis.document = previousDocument;
    }

    assert.equal(appendedScript.src, "https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX&gtm_auth=auth-token");
    assert.deepEqual(appendedScript.attributes, { "data-consent-category": "google" });
});

test("initialize accepts an options object payload", () => {
    let appendedScript = null;
    const previousWindow = globalThis.window;
    const previousDocument = globalThis.document;

    globalThis.window = {};
    globalThis.document = {
        createElement(tagName) {
            return {
                tagName,
                attributes: {},
                setAttribute(name, value) {
                    this.attributes[name] = value;
                }
            };
        },
        getElementsByTagName() {
            return [{
                appendChild(node) {
                    appendedScript = node;
                }
            }];
        }
    };

    try {
        gtmModule.initialize({
            url: "https://www.googletagmanager.com",
            gtmId: "GTM-YYYYYYY",
            attributes: { nonce: "abc123" },
            debugToConsole: false,
            queryParameters: { gtm_preview: "env-9" }
        });
    } finally {
        globalThis.window = previousWindow;
        globalThis.document = previousDocument;
    }

    assert.equal(appendedScript.src, "https://www.googletagmanager.com/gtm.js?id=GTM-YYYYYYY&gtm_preview=env-9");
    assert.deepEqual(appendedScript.attributes, { nonce: "abc123" });
});
