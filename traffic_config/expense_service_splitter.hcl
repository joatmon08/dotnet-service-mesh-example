# Canary Release
kind = "service-splitter",
name = "expense"

splits = [
  {
    weight = 50,
    service_subset = "v1"
  },
  {
    weight = 50,
    service_subset = "v2"
  }
]