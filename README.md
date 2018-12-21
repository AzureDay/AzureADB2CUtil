# About

Util to collect Azure AD B2C users

# How to use

## 1. Create new Azure Web App

## 2. Fork this repo to your GitHub account 

## 3. Set up continuous deployment from GitHub

https://docs.microsoft.com/en-us/azure/app-service/app-service-continuous-deployment#deploy-continuously-from-github

## 4. Set application environment variables 

    "ADB2C:ClientId" - Your Azure AD Application ClientId
    "ADB2C:ClientSecret" - Your Azure AD Application ClientSecret
    "ADB2C:Tenant" - Your Azure AD Tenant

## 5. Setup Azure Web App Authentication and authorization 

https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-overview#authorization-behavior
