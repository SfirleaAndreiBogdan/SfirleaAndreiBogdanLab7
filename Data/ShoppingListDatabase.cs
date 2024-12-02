using SfirleaAndreiBogdanLab7.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfirleaAndreiBogdanLab7.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database;
        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Products>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
        }

        public Task<int> SaveProductAsync(Products product)
        {
            if (product.ID != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);
            }
        }
        public Task<int> DeleteProductAsync(Products product)
        {
            return _database.DeleteAsync(product);
        }
        public Task<List<Products>> GetProductsAsync()
        {
            return _database.Table<Products>().ToListAsync();
        }
public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>()
            .Where(i => i.ID == id)
           .FirstOrDefaultAsync();
        }

        public async Task<List<Products>> GetProductsForShopListAsync(int shopListID)
        {
            
            var listProducts = await _database.Table<ListProduct>()
                                               .Where(lp => lp.ShopListID == shopListID)
                                               .ToListAsync();

           
            var productIDs = listProducts.Select(lp => lp.ProductID).ToList();

            
            var products = await _database.Table<Products>()
                                           .Where(p => productIDs.Contains(p.ID))
                                           .ToListAsync();

            return products;
        }


        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);
            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }
        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(slist);
        }

        public Task<int> SaveListProductAsync(ListProduct listp)
        {
            if (listp.ID != 0)
            {
                return _database.UpdateAsync(listp);
            }
            else
            {
                return _database.InsertAsync(listp);
            }
        }
        public Task<List<Products>> GetListProductsAsync(int shoplistid)
        {
            return _database.QueryAsync<Products>(
            "select P.ID, P.Description from Products P"
            + " inner join ListProduct LP"
            + " on P.ID = LP.ProductID where LP.ShopListID = ?",
            shoplistid);
        }
    }
}
