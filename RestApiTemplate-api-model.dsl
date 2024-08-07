workspace {

    model {
        user = person "User" "Pessoa que utiliza a API para gerenciar as vendas."

        softwareSystem = softwareSystem "Sistema de Vemdas API" "API para gerenciar as vendas de produtos." {
            apiContainer = container "API REST (.NET Core)" "Exponha operações REST para gerenciar vendas." "C# .NET Core" {
                user -> this "Utiliza"

                salesController = component "ProductsController" "Controlador para operações de vendas." "C#"
                createProductRequest = component "CreateProductRequest" "Request a ser resolvida a lógica de negócios para criação de uma venda." "C#"
                getProductRequest = component "GetProductRequest" "Request a ser resolvida a lógica de negócios para a busca de uma venda por identificador." "C#"
                updateProductRequest = component "UpdateProductRequest" "Request a ser resolvida a lógica de negócios de atualização do status de uma venda." "C#"
                repository = component "Repository" "Repositório para persistência das vendas." "C#"
                inMemoryDatabase = component "InMemoryDatabase" "Banco de dados em memória para armazenar as vendas (Armazena no arquivo Products.json)." "In-Memory Database"

                salesController -> createProductRequest "Chama o request"
                salesController -> getProductRequest "Chama o request"
                salesController -> updateProductRequest "Chama o request"
                createProductRequest -> repository "Chama o request"
                getProductRequest -> repository "Chama o request"
                updateProductRequest -> repository "Chama o request"
                repository -> inMemoryDatabase "Interage"
                user -> salesController "Interage com"
            }

            swaggerContainer = container "Swagger UI" "Interface para documentação da API." "Swagger" {
                apiContainer -> this "Fornece documentação"
            }
            
            database = container "InMemoryDatabase" "Banco de dados em memória para armazenar as vendas (Armazena no arquivo Products.json)." "In-Memory Database" {
                apiContainer -> this "Persiste dados"
                
                sale = component "Product" "Representa uma sale de produtos." "C#"
                vendedor = component "Vendedor" "Representa um vendedor." "C#"
                item = component "Item" "Representa um item vendido." "C#"
                statusProduct = component "StatusProduct" "Representa o status de uma sale." "C#"
                
                sale -> vendedor "Contém" "1..*"
                sale -> item "Possui" "*..1"
                sale -> statusProduct "Tem" "1"
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
