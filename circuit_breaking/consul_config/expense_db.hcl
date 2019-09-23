service {
  name    = "expense-db"
  id      = "expense-db"
  address = "10.5.0.3"
  port    = 1433

  connect {
    sidecar_service {
      port = 19000

      check {
        name     = "Connect Envoy Sidecar"
        tcp      = "10.5.0.3:19000"
        interval = "10s"
      }

      proxy {}
    }
  }
}