locals {
  src_folder_name       = basename(var.src_path)
  function_publish_path = "${var.src_path}/bin/Release/net6.0/publish"
  function_zip_path     = "${var.src_path}/bin/Release/net6.0/${local.src_folder_name}.zip"
  src_filenames         = fileset(var.src_path, "**/*")
}

resource "null_resource" "BuildLambdaScript" {
  triggers = {
    dir_sha1 = base64sha256(join("", toset([
      for file in local.src_filenames : filebase64sha256("${var.src_path}/${file}") if length(regexall("(.*bin/.*)|(.*obj/.*)|(.*bin\\.*)|(.*obj\\.*)", file)) == 0
    ])))
  }

  provisioner "local-exec" {
    command     = "dotnet lambda package"
    working_dir = var.src_path
  }
}

data "archive_file" "LambdaArchive" {
  depends_on = [
    null_resource.BuildLambdaScript
  ]
  type        = "zip"
  source_dir  = local.function_publish_path
  output_path = local.function_zip_path
}
