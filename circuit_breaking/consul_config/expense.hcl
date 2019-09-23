service {
  name    = "expense"
  id      = "expense"
  address = "10.5.0.4"
  port    = 5001

  connect {
    sidecar_service {
      port = 19000

      check {
        name     = "Connect Envoy Sidecar"
        tcp      = "10.5.0.4:19000"
        interval = "10s"
      }

      proxy {
        upstreams {
          destination_name   = "expense-db"
          local_bind_address = "127.0.0.1"
          local_bind_port    = 1433
        }
      }
    }
  }
}