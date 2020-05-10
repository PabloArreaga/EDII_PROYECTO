using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Huffman;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Controllers
{
    delegate string ToString(object obj);
    delegate object ToObject(string obj);
    delegate object Edit(object obj, string[] txt);

    [ApiController]
    [Route("api/[controller]")]

    public class GroceryStoreController : ControllerBase
    {
        //[Route("TransferStore-Products")]
        //[HttpGet]
        //public List<List<Comp_Store_Product>> getTransferStoreProducts([FromForm]int product, [FromForm] int transmitter, [FromForm]int receiver, [FromForm] int stock)
        //{
        //    BTree<Comp_Store_Product>.Create("TreeStoreProduct", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
        //    var nodoTransmiter = BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = transmitter, _idProduct = product }, true);
        //    var nodoReceiver = BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = receiver, _idProduct = product }, true);
        //    if (nodoTransmiter.Count != 0 && nodoTransmiter[0]._stock - stock >= 0)
        //    {
        //        if (nodoReceiver.Count == 0)
        //        {
        //            BTree<Comp_Store_Product>.ValidateIncert(new Comp_Store_Product { _idStore = receiver, _idProduct = product, _stock = stock });
        //        }
        //        else
        //        {
        //            nodoReceiver[0]._stock = nodoReceiver[0]._stock + stock;
        //            BTree<Comp_Store_Product>.ValidateEdit(nodoReceiver[0], new string[2] { nodoReceiver[0]._stock.ToString(), string.Empty }, new Edit(Comp_Store_Product.Modify));
        //        }
        //        nodoTransmiter[0]._stock = nodoTransmiter[0]._stock - stock;
        //        BTree<Comp_Store_Product>.ValidateEdit(nodoTransmiter[0], new string[2] { nodoTransmiter[0]._stock.ToString(), string.Empty }, new Edit(Comp_Store_Product.Modify));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    var nodoList = new List<List<Comp_Store_Product>>();
        //    nodoList.Add(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = transmitter, _idProduct = product }, true));
        //    nodoList.Add(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = receiver, _idProduct = product }, true));
        //    return nodoList;
        //}
    }
}
