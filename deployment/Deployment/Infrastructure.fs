﻿module Deployment.Infrastructure

open Farmer
open Farmer.Builders
open Farmer.CoreTypes

open Common.StringHelper

let createDeployment appName = 
    let storageName = appName |> append "-storage" |> remove "-"
    let servicePlanName = appName |> append "-web-host"
    let appInsightsName = appName |> append "-insights"

    let storage = storageAccount {
        name storageName
        sku Storage.Sku.Standard_LRS
    }

    let webApp = webApp {
        name appName
        service_plan_name servicePlanName
        sku WebApp.Sku.B1
        always_on
        link_to_unmanaged_app_insights (ResourceId.create appInsightsName)
        setting "public_path" "./public"
        setting "STORAGE_CONNECTIONSTRING" storage.Key
        setting "ASPNETCORE_ENVIRONMENT" "azure"
    }

    arm {
        location Location.NorthEurope
        add_resource storage
        add_resource webApp
    }
