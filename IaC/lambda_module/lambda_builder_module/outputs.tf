output "zip_path" {
  value = local.function_zip_path
}

output "zip_hash" {
  value = filebase64sha256(local.function_zip_path)
}