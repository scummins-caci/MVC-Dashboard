(function () {
    'use strict';

    // Configure RequireJS to shim Jasmine
    requirejs.config({
        baseUrl: "../../Scripts/App",
        paths: {
            'jasmine': '../../Testing/Jasmine/lib/jasmine',
            'jasmine-html': '../../Testing/Jasmine/lib/jasmine-html',
            'boot': '../../Testing/Jasmine/lib/boot',
            'spec': '../../Testing/Spec',
            'squire': '../../Testing/Jasmine/Squire',
            'ko': '../../Scripts/knockout-3.2.0',
            'modal': '../../Scripts/bootstrapko-modal'
        },
        shim: {
            'jasmine': {
                exports: 'jasmine'
            },
            'jasmine-html': {
                deps: ['jasmine'],
                exports: 'jasmine'
            },
            'boot': {
                deps: ['jasmine', 'jasmine-html'],
                exports: 'jasmine'
            },
            "squire": {
                exports: "squire"
            }
        }
    });

    // Define all of your specs here. These are RequireJS modules.
    var specs = [
      'spec/NotificationsSpec',
      'spec/WorkflowServicesSpec',
      'spec/DataflowReceiptsSpec',
      'spec/WorkitemCountsSpec',
      'spec/WorkitemStatusSpec',
      'spec/SearchAuditSpec'
    ];

    // Load Jasmine - This will still create all of the normal Jasmine browser globals unless `boot.js` is re-written to use the
    // AMD or UMD specs. `boot.js` will do a bunch of configuration and attach it's initializers to `window.onload()`. Because
    // we are using RequireJS `window.onload()` has already been triggered so we have to manually call it again. This will
    // initialize the HTML Reporter and execute the environment.
    require(['boot'], function () {

        // Load the specs
        require(specs, function () {

            // Initialize the HTML Reporter and execute the environment (setup by `boot.js`)
            window.onload();
        });
    });

})();