import { InfoDialog } from './../components/infodialog/index';
import {
  inject,
  CompositionTransaction,
  CompositionTransactionNotifier,
} from "aurelia-framework";
import { Router } from "aurelia-router";
import { EventAggregator } from "aurelia-event-aggregator";
import { HahnService } from "../services/hahnService";
import { User as User } from "../entities/user";
import { DialogService } from "aurelia-dialog";
import { ConfirmDialog } from "../components/confirmdialog";

import { I18N } from "aurelia-i18n";

import {
  ValidationControllerFactory,
  ValidationRules,
} from "aurelia-validation";

import { BootstrapFormRenderer } from "../resources/bootstrap-form-renderer";

@inject(
  HahnService,
  ValidationControllerFactory,
  CompositionTransaction,
  Router,
  I18N,
  DialogService,
  EventAggregator,
  User
)
export class Profile {
  private service: HahnService;
  private controllerFactory: ValidationControllerFactory;
  private notifier: CompositionTransactionNotifier;
  private router: Router;
  private i18n: I18N;
  private dialogService: DialogService;
  private ea: EventAggregator;
  controller = null;
  title: any;
  validation: any;
  standardGetMessage: any;
  profile: any;

  constructor(
    service,
    controllerFactory,
    compositionTransaction,
    router,
    i18n,
    dialogService,
    ea,
    private user: User
    ) {
    this.service = service;
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer());
    this.notifier = compositionTransaction.enlist();
    this.router = router;
    this.i18n = i18n;
    this.dialogService = dialogService;
    this.ea = ea;
    this.profile = user;
    this.title = i18n.tr('lblProfile');
  }
  attached(): void {}
  action(): void {
    this.clearData();
  }

  public clearData() {
    this.dialogService
      .open({
        viewModel: ConfirmDialog,
        model: "Are you sure to reset",
      })
      .whenClosed()
      .then((response) => {
        console.log(response);
        if(!response.wasCancelled)
          this.profile = null;
      });
  }

  get canSave() {
    return this.user != null && (
      this.user.firstname &&
      this.user.lastname &&
      this.user.email &&
      this.user.age && this.user.age >= 18 &&
      this.user.address      
    );
  }

  get canReset() {
    return this.user != null && (
      this.user.firstname ||
      this.user.lastname ||
      this.user.email ||
      this.user.age ||
      this.user.address
    );
  }

  activate = async () => {
    try {
      this.notifier.done();
      await this.setupValidation();
    } catch (error) {
      console.log(error);
    }
  };

  setupValidation() {

    ValidationRules.ensure("firstname")
      .displayName("First Name")
      .required()
      .minLength(3)
      .withMessage("First Name must be at least 3 characters")

      .ensure("lastname")
      .displayName("Last Name")
      .required()
      .minLength(3)
      .withMessage("Last Name must be at least 3 characters")

      .ensure("email")
      .required()
      .email()
      .withMessage("Email is required")

      .ensure("age")
      .required()
      .min(19)
      .withMessage("Age must be greater then 18")

      .ensure("address.housenumber")
      .displayName("housenumber")
      .required()
      .withMessage("House Number is required")

      .ensure("address.street")
      .displayName("street")
      .required()
      .withMessage("Street is required")

      .ensure("address.postalcode")
      .displayName("postalcode")
      .required()
      .withMessage("Postal Code is required")

      .on(this.user);
  }

  validateCreateProfile = async () => {
    try {
      var res = await this.controller.validate();
      if (res.valid) {
        this.createProfile();
      }
    } catch (error) {
      console.log(error);
    }
  };

  createProfile = async () => {
    try {
      await this.service
        .createProfile(this.user)
        .then((response) => {
          debugger;
          if(response && response.id) {
            localStorage.setItem("userId",  response.id);
            this.router.navigateToRoute("Home");
          }
          else
          {
            let message = response.errors ? JSON.stringify(response.errors) : "Error saving profile."; 
            this.dialogService
              .open({
                viewModel: InfoDialog,
                model: message,
              });
          }
        });
    } catch (error) {
      console.log(error);
    }
  };
}
