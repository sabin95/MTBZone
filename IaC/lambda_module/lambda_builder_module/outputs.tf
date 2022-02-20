output "zip_path" {
  value = local.function_zip_path
}

output "zip_hash" {
  value = data.archive_file.LambdaArchive.output_base64sha256
}
