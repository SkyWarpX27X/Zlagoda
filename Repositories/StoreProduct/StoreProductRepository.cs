using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.StoreProduct;

public class StoreProductRepository : IStoreProductRepository
{
    private readonly SqliteConnection _connection;
    public StoreProductRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static StoreProductInfoDataModel MapStoreProduct(SqliteDataReader reader) => new(
        reader.GetString(reader.GetOrdinal("UPC")),
            reader.GetString(reader.GetOrdinal("product_name")),
        reader.IsDBNull(reader.GetOrdinal("UPC_prom")) ? null : reader.GetString(reader.GetOrdinal("UPC_prom")),
        reader.GetInt64(reader.GetOrdinal("id_product")),
        reader.GetDecimal(reader.GetOrdinal("selling_price")),
        reader.GetInt32(reader.GetOrdinal("products_number")),
        reader.GetBoolean(reader.GetOrdinal("promotional_product"))
    );

    public StoreProductInfoDataModel? GetStoreProduct(string upc)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Store_Product S JOIN Product P ON S.id_product = P.id_product WHERE UPC = @upc";
        command.Parameters.AddWithValue("@upc", upc);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapStoreProduct(reader) : null;
    }
    
    public IEnumerable<StoreProductInfoDataModel> GetStoreProducts(bool sortByName = true, bool sortByQuantity = false)
    {
        using var command = _connection.CreateCommand();
        // Since we join, it technically returns all attributes. Is that necessarily bad? I'm not sure, since I think
        // SELECT basically just does a projection on the final result anyway, and we just ignore that by mapping it to
        // the DB model.
        var query = "SELECT * FROM Store_Product JOIN Product ON Store_Product.id_product = Product.id_product";
        if (sortByName || sortByQuantity) 
            query += $"ORDER BY {(sortByName ? "product_name" : "products_number")}";
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }

    public IEnumerable<StoreProductInfoDataModel> GetStoreProductsPromotional(bool sortByName = true, bool sortByQuantity = false)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Store_Product JOIN Product ON Store_Product.id_product = Product.id_product";
        if (sortByName || sortByQuantity) 
            query += $"""
                      WHERE promotional_product IS TRUE
                      ORDER BY {(sortByName ? "product_name" : "products_number")}
                      """;
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }
    
    public IEnumerable<StoreProductInfoDataModel> GetStoreProductsNonPromotional(bool sortByName = true, bool sortByQuantity = false)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Store_Product JOIN Product ON Store_Product.id_product = Product.id_product";
        if (sortByName || sortByQuantity) 
            query += $"""
                      WHERE promotional_product IS FALSE
                      ORDER BY {(sortByName ? "product_name" : "products_number")}
                      """;
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }

    public StoreProductBriefInfoDataModel? GetStoreProductBriefInfo(string upc)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT selling_price, products_number, product_name, characteristics
                              FROM Store_Product JOIN Product ON Store_Product.id_product = Product.id_product
                              WHERE UPC = @upc;
                              """;
        using var reader = command.ExecuteReader();
        return !reader.Read()
            ? null
            : new StoreProductBriefInfoDataModel(upc,
                reader.GetDecimal(reader.GetOrdinal("selling_price")),
                reader.GetInt32(reader.GetOrdinal("products_number")),
                reader.GetString(reader.GetOrdinal("product_name")),
                reader.GetString(reader.GetOrdinal("characteristics")));
    }
    
    public void AddStoreProduct(StoreProductDBModel storeProduct)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Store_Product (UPC, UPC_prom, id_product, selling_price, products_number, promotional_product) 
                              VALUES (@UPC,  @UPC_prom, @id_product, @selling_price, @products_number, @promotional_product)
                              """;
        command.Parameters.AddWithValue("@UPC", storeProduct.UPC);
        command.Parameters.AddWithValue("@UPC_prom", storeProduct.UPCProm is null ? DBNull.Value : storeProduct.UPCProm);
        command.Parameters.AddWithValue("@id_product", storeProduct.ProductId);
        command.Parameters.AddWithValue("@selling_price", storeProduct.SellingPrice);
        command.Parameters.AddWithValue("@products_number", storeProduct.Quantity);
        command.Parameters.AddWithValue("@promotional_product", storeProduct.Promotional);
        command.ExecuteNonQuery();
    }

    public void UpdateStoreProduct(StoreProductDBModel storeProduct)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              UPDATE Store_Product 
                              SET 
                                UPC_prom = @UPC_prom,
                                id_product = @id_product,
                                selling_price = @selling_price,
                                products_number = @products_number,
                                promotional_product = @promotional_product
                              WHERE
                                  UPC = @UPC;
                              """;
        command.Parameters.AddWithValue("@UPC_prom", storeProduct.UPCProm);
        command.Parameters.AddWithValue("@id_product", storeProduct.ProductId);
        command.Parameters.AddWithValue("@selling_price", storeProduct.SellingPrice);
        command.Parameters.AddWithValue("@products_number", storeProduct.Quantity);
        command.Parameters.AddWithValue("@promotional_product", storeProduct.Promotional);
        command.Parameters.AddWithValue("@UPC", storeProduct.UPC);
        command.ExecuteNonQuery();
    }

    public void DeleteStoreProduct(string upc)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Store_Product WHERE UPC = @UPC;";
        command.Parameters.AddWithValue("@UPC", upc);
        command.ExecuteNonQuery();
    }
    
    
}