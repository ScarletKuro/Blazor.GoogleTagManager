export function initialize(url, GTMID, attributes, debugToConsole = false) {
    (function (w, d, s, l, u, i, m, k) {
    	w[l] = w[l] || [];
    	w[l].push({
    		"gtm.start": new Date().getTime(),
    		event: "gtm.js"
        });
        var f = d.getElementsByTagName("head")[0],
    		j = d.createElement(s),
            dl = l !== "dataLayer" ? "&l=" + l : "";
        j.async = true;
        j.src = u + "/gtm.js?id=" + i + dl;
        for (const [key, value] of Object.entries(m)) {
            j.setAttribute(key, value);
        }
        f.appendChild(j, f);
    	window.dataLayer.push({ event: "pageview" });
        window.isGTM = true;
        if (k) {
            console.log("[GTM]: Configured with URL = " + u + ", and " + "GtmId = " + i);
        }
    })(window, document, "script", "dataLayer", url, GTMID, attributes, debugToConsole);
}

export function push(data, debugToConsole = false) {
    window.dataLayer.push(data);
    if (debugToConsole) {
        console.log("[GTM]:" + JSON.stringify(data));
    }
}

export function pushEvent(eventName, eventData, debugToConsole = false) {
	if (eventData === null) {
		eventData = new Object();
	}
	eventData['event'] = eventName;
    push(eventData, debugToConsole);
}

export function pushPageViewEvent(eventName, urlVariableName, url, eventData, debugToConsole = false) {
	if (eventData === null) {
		eventData = new Object();
	}
	eventData[urlVariableName] = url;
	eventData['event'] = eventName;
    push(eventData, debugToConsole);
}