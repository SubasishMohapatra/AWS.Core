# Introduction 
This repository will have core packages of some of the common AWS components for the purpose fo reusablity and code maintainability

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
Ad the nuget package by using the Nuget package manager or Package console
2.	Software dependencies
Following Nuget packages are used:
    <PackageReference Include="AWSSDK.S3" Version="3.7.103.45" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="7.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="7.0.4" />
    <PackageReference Include="Microsoft.Extensions.Options" Version="7.0.1" />
    <PackageReference Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="7.0.0" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="System.Text.Json" Version="7.0.2" />
3.	Latest releases
1.0.0
4.	API references
https://docs.aws.amazon.com/sdk-for-net/index.html

# Build and Test
Add this core nuget package to the client and add config settings as below to appsettings.json: 
"S3ConfigOptions": {
    "S3Region": "MANDATORY: region",
    "AccountId": "MANDATORY: AccountId",
    "BucketName": "MANDATORY: bucketName",
    "IamAccessKey": "OPTIONAL credentials but needed for local development",
    "IamSecretKey": "OPTIONAL credentials but needed for local development"
  },

# Contribute
Feel free to clone the source code and raise a PR to merge for critical and important functionalities
