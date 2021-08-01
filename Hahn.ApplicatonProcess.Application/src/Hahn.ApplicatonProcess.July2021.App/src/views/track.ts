import { TrackData } from '../entities/trackData';
import { inject, bindable } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { Router } from "aurelia-router";
import { HahnService } from "../services/hahnService";
import { DialogService } from "aurelia-dialog";
import { ConfirmDialog } from "../components/confirmdialog";
import { I18N } from "aurelia-i18n";

@inject(Router, HahnService, DialogService, I18N, EventAggregator)
export class TrackAssets {
  private i18n = [I18N];
  private service: HahnService;
  private router: Router;
  private dlg: DialogService;
  private ea: EventAggregator;
  title: any;
  remoteassets: [];

  tracked: TrackData;

  constructor(router, service, dlg, i18n, ea) {
    this.router = router;
    this.service = service;
    this.dlg = dlg;
    this.i18n = i18n;
    this.ea = ea;
    this.title = i18n.tr('lblTrack');
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
      .getAssetsRemote()
      .then((response) => (this.remoteassets = response));
    return true;
  }

  trackAsset = async (assetId, trackUntrack) => {
    
    let message = (trackUntrack ? 'Tracking ' : 'Un-Tracking ') + assetId;
    let data = new TrackData({
        assetId: assetId,
        userId: localStorage.getItem("userId"),
        trackUntrack: trackUntrack
    });
    await this.service.trackAsset(data);
    alert(message);
  };
}
