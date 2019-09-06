terragrunt {
  # Terragrunt will copy the Terraform configurations specified by the source parameter, along with any files in the
  # working directory, into a temporary folder, and execute your Terraform commands in that folder.
  terraform {
    source = "git::https://github.com/dfds/infrastructure-modules.git//compute/ecr-repo"
  }

  # Include all settings from the root terraform.tfvars file
  include = {
    path = "${find_in_parent_folders()}"
  }
}

name = "harald/harald-dbmigrations"
pull_principals = ["arn:aws:iam::738063116313:root"] #dfds-oxygen