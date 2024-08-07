workspace {

    model {
        user = person "User" "Pessoa que utiliza a API."

        softwareSystem = softwareSystem "RestApiTemplate API" "API para gerenciar RestApiTemplate." {
            apiContainer = container "API REST (.NET Core)" "Exponha operações REST para gerenciar RestApiTemplate." "C# .NET Core" {
                user -> this "Utiliza"

                productsController = component "ProductsController" "Controlador para gestão de produtos." "C#"
                createProductRequest = component "CreateProductRequest" "Request a ser resolvida a lógica de negócios para criação de um produto." "C#"
                getProductRequest = component "GetProductRequest" "Request a ser resolvida a lógica de negócios para a busca de um produto por identificador." "C#"
                updateProductRequest = component "UpdateProductRequest" "Request a ser resolvida a lógica de negócios de atualização do status de um produto." "C#"
                repository = component "Repository" "Repositório para persistência das vendas." "C#"
                inMemoryDatabase = component "InMemoryDatabase" "Banco de dados em memória para armazenar as vendas (Armazena no arquivo Products.json)." "In-Memory Database"

                productsController -> createProductRequest "Chama o request"
                productsController -> getProductRequest "Chama o request"
                productsController -> updateProductRequest "Chama o request"
                createProductRequest -> repository "Chama o request"
                getProductRequest -> repository "Chama o request"
                updateProductRequest -> repository "Chama o request"
                repository -> inMemoryDatabase "Interage"
                user -> productsController "Interage com"
            }

            swaggerContainer = container "Swagger UI" "Interface para documentação da API." "Swagger" {
                apiContainer -> this "Fornece documentação"
            }
            
            database = container "InMemoryDatabase" "Banco de dados em memória para armazenar as vendas (Armazena no arquivo Products.json)." "In-Memory Database" {
                apiContainer -> this "Persiste dados"
                
                product = component "Product" "Representa um produtos." "C#"
                statusProduct = component "StatusProduct" "Representa o status de um produto." "C#"
                
                product -> vendedor "Contém" "1..*"
                product -> item "Possui" "*..1"
                product -> statusProduct "Tem" "1"
            }
        }
    }

    views {
        systemContext softwareSystem {
            include *
            autolayout lr
        }

        container softwareSystem {
            include *
            autolayout lr
        }

        component apiContainer {
            include *
            autolayout lr
        }

        component database {
            include *
            autolayout lr
        }

        theme default
    }

}
