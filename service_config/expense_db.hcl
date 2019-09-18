service {
  name    = "expense-db"
  id      = "expense-db"
  address = "10.5.0.3"
  port    = 1433

  tags = ["v1"]
  meta = {
    version = "1"
  }

  connect {
    sidecar_service {
      port = 20000

      check {
        name     = "Connect Envoy Sidecar"
        tcp      = "10.5.0.3:20000"
        interval = "10s"
      }

      proxy {
        config {
          envoy_local_cluster_json = <<EOL
           {
             "@type": "type.googleapis.com/envoy.api.v2.Cluster",
             "name": "local_app",
             "connect_timeout": "5s",
             "circuit_breakers": {
               "thresholds": [
                 {
                   "priority": "HIGH",
                   "max_requests": 1
                 }
               ]
             },
             "load_assignment": {
              "cluster_name": "local_app",
              "endpoints": [
               {
                "lb_endpoints": [
                 {
                  "endpoint": {
                   "address": {
                    "socket_address": {
                     "address": "127.0.0.1",
                     "port_value": 1433
                    }
                   }
                  }
                 }
                ]
               }
              ]
             }
           }
          EOL
        }
      }
    }
  }
}