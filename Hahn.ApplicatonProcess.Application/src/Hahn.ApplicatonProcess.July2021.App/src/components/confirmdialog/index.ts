import { autoinject } from "aurelia-framework";
import { DialogController } from "aurelia-dialog";

@autoinject
export class ConfirmDialog {
  constructor(public controller: DialogController) {
    controller.settings.centerHorizontalOnly = true;
  }
  message = "";
  activate(data) {
    this.message = data;
  }
  ok(): void {
    this.controller.ok();
  }
  cancel(): void {
    this.controller.cancel();
  }
}
