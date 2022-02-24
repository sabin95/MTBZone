terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "3.74.0"
    }
    http = {
      source  = "hashicorp/http"
      version = "2.1.0"
    }
    null = {
      source  = "hashicorp/null"
      version = "3.1.0"
    }
    archive = {
      source  = "hashicorp/archive"
      version = "2.2.0"
    }
  }
}

provider "null" {
}

provider "archive" {
}

provider "http" {
}

provider "aws" {
  region  = "eu-central-1"
}
