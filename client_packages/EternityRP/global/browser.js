﻿let customBrowser = undefined;
let parameters = null;

const buildEvent = function(event, data) {
	return JSON.stringify({ name, data })
}

const sendEvent = function(json) {
	if(customBrowser) customBrowser.execute(`app.event('${json}')`)
}

mp.events.add('createBrowser', (page, event, data) => {
	if(customBrowser === undefined) {
		if(event) {
			parameters = buildEvent(event, data)
		}
		parameters = null
		customBrowser = mp.browsers.new(page);
	}
});

mp.events.add('browserDomReady', browser => {
	if(customBrowser === browser) {
		// Enable the cursor
		mp.gui.cursor.visible = true;

		if(parameters) {
			sendEvent(parameters);
		}
	}
});

mp.events.add('sendEvent', (name, data) => {
	sendEvent(buildEvent(name, data))
});

mp.events.add('destroyBrowser', () => {
	// Disable the cursor
	mp.gui.cursor.visible = false;

	// Destroy the browser
	customBrowser.destroy();
	customBrowser = undefined;
});
