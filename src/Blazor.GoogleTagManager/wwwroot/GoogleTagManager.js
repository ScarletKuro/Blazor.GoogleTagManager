export function initialize(GTMID, attributes) {
    (function (w, d, s, l, i, m) {
    	w[l] = w[l] || [];
    	w[l].push({
    		"gtm.start": new Date().getTime(),
    		event: "gtm.js"
        });
        var f = d.getElementsByTagName("head")[0],
    		j = d.createElement(s),
            dl = l !== "dataLayer" ? "&l=" + l : "";
        j.async = true;
        j.src = "https://www.googletagmanager.com/gtm.js?id=" + i + dl;
        for (const [key, value] of Object.entries(m)) {
            j.setAttribute(key, value);
        }
        f.appendChild(j, f);
    	window.dataLayer.push({ event: "pageview" });
    	window.isGTM = true;
    })(window, document, "script", "dataLayer", GTMID, attributes);
}

export function push(data) {
	window.dataLayer.push(data);
	console.debug("GTM:" + JSON.stringify(data));
}

export function pushEvent(eventName, eventData) {
	if (eventData === null) {
		eventData = new Object();
	}
	eventData['event'] = eventName;
	push(eventData);
}

export function pushPageViewEvent(eventName, urlVariableName, url, eventData) {
	if (eventData === null) {
		eventData = new Object();
	}
	eventData[urlVariableName] = url;
	eventData['event'] = eventName;
	push(eventData);
}