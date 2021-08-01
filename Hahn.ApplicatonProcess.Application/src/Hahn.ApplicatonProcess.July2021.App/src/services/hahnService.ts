import { HttpClient, json } from "aurelia-fetch-client";
import { inject } from "aurelia-framework";
import { api } from "../config/api";

@inject(HttpClient)
export class HahnService {
  private http: HttpClient;

  constructor(http: HttpClient) {
    http.configure((config) => {
      config
        .withBaseUrl(api.baseUrl)
        .withDefaults({
          credentials: "same-origin",
          headers: {
            Accept: "application/json",
            "X-Requested-With": "Fetch",
          },
        })
        .withInterceptor({
          request(request) {
            console.log(`Requesting ${request.method} ${request.url}`);
            return request; // you can return a modified Request, or you can short-circuit the request by returning a Response
          },
          response(response) {
            console.log(`Received ${response.status} ${response.url}`);
            return response; // you can return a modified Response
          },
        });
    });
    this.http = http;
  }

  createProfile(profile) {
    return this.http
      .fetch("users", {
        method: "post",
        body: json(profile),
      })
      .then((response) => response.json())
      .then((response) => {
        console.log(response);
        return response;
      })
      .catch((error) => {
        console.log("Error saving profile", error);
      });
  }

  getProfile(id) {
    return this.http
      .fetch(`users/${id}`, {
        method: "get",
      })
      .then((response) => response.json())
      .then((liveassets) => {
        return liveassets;
      })
      .catch((error) => {
        console.log("Error retrieving live assets.", error);
        return [];
      });
  }

  getAssetsRemote() {
    return this.http
      .fetch("assets/remote", {
        method: "get",
      })
      .then((response) => response.json())
      .then((remoteassets) => {
        console.log("Fetching... ", remoteassets);
        return remoteassets;
      })
      .catch((error) => {
        console.log("Error retrieving remoteassets.", error);
        return [];
      });
  }

  trackAsset(trackdata) {
    return this.http
      .fetch(`users/trackasset`, {
        method: "post",
        body: json(trackdata),
      })
      .then((response) => response.json())
      .then((result) => {
        return result;
      })
      .catch((error) => {
        console.log("Error tracking assets.", error);
        return [];
      });
  }

  getLiveAssets(id) {
    return this.http
      .fetch(`users/${id}/liveassets`, {
        method: "get",
      })
      .then((response) => response.json())
      .then((liveassets) => {
        return liveassets;
      })
      .catch((error) => {
        console.log("Error retrieving live assets.", error);
        return [];
      });
  }

}
