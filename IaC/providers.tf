terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "3.74.0"
    }
    http = {
      source = "hashicorp/http"
      version = "2.1.0"
    }
  }
}

provider "http" {
}

provider "aws" {
  region = "eu-central-1"
}