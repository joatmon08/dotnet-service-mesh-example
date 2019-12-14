service {
  name    = "expense"
  id      = "expense"
  address = "10.5.0.4"
  port    = 5001

  connect {
    sidecar_service {
      port = 20000

      check {
        name     = "Connect Envoy Sidecar"
        tcp      = "10.5.0.4:20000"
        interval = "10s"
      }
      

      proxy {
        upstreams {
          destination_name   = "database"
          local_bind_port    = 1433
        }
      }
    }
  }
}