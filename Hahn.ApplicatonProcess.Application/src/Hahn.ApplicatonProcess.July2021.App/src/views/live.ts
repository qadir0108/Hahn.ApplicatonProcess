import { inject, bindable } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { Router } from "aurelia-router";
import { HahnService } from "../services/hahnService";
import { DialogService } from "aurelia-dialog";
import { ConfirmDialog } from "../components/confirmdialog";
import { I18N } from "aurelia-i18n";

@inject(Router, HahnService, DialogService, I18N, EventAggregator)
export class LiveAssets {
  private i18n = [I18N];
  private service: HahnService;
  private router: Router;
  private dlg: DialogService;
  private ea: EventAggregator;
  title: any;
  liveassets: [];

  constructor(router, service, dlg, i18n, ea) {
    this.router = router;
    this.service = service;
    this.dlg = dlg;
    this.i18n = i18n;
    this.ea = ea;
    this.title = i18n.tr('lblLive');
  }

  activate() {
    try {
      this.load();
    } catch (error) {
      console.log(error);
    }
  }

  async load() {
    await this.service
      .getLiveAssets(localStorage.getItem("userId"))
      .then((response) => (this.liveassets = response));
    console.log(this.liveassets);
  }

}
