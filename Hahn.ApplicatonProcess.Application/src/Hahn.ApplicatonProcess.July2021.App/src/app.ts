import { PLATFORM } from "aurelia-framework";
import { Router, RouterConfiguration } from "aurelia-router";
import { I18N } from "aurelia-i18n";
import { inject } from "aurelia-framework";

@inject(I18N)
export class App {

  private i18n = [I18N];
  constructor(i18n) {
    this.i18n = i18n;
    i18n
        .setLocale('de')
        .then( () => {
    });
  }

  router: Router | undefined;
  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Hahn Asset Tracker";
    config.map([
      { route: ['', 'live'],       name: 'LiveAssets',       moduleId: PLATFORM.moduleName('views/live'),    nav: true, title: 'Live Assets' },
      { route: 'profile',          name: 'CreateProfile',    moduleId: PLATFORM.moduleName('views/profile'), nav: true, title: 'Create Profile' },
      { route: 'home',             name: 'Home',             moduleId: PLATFORM.moduleName('views/home'),    nav: true, title: 'Home' },
      { route: 'track',            name: 'TrackAssets',      moduleId: PLATFORM.moduleName('views/track'),   nav: true, title: 'Track Assets' }
    ]);
    
    this.router = router;
  }
}
