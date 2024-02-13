﻿using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class Item
{
    public string? BinLocation { get; set; }

    public decimal BuydownPrice { get; set; }

    public double BuydownQuantity { get; set; }

    public decimal CommissionAmount { get; set; }

    public decimal CommissionMaximum { get; set; }

    public int CommissionMode { get; set; }

    public float CommissionPercentProfit { get; set; }

    public float CommissionPercentSale { get; set; }

    public string? Description { get; set; }

    public bool FoodStampable { get; set; }

    public int Hqid { get; set; }

    public bool ItemNotDiscountable { get; set; }

    public DateTime? LastReceived { get; set; }

    public DateTime LastUpdated { get; set; }

    public double QuantityCommitted { get; set; }

    public int SerialNumberCount { get; set; }

    public double TareWeightPercent { get; set; }

    public int Storeid { get; set; }

    public int Id { get; set; }

    public string ItemLookupCode { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int CategoryId { get; set; }

    public int MessageId { get; set; }

    public decimal Price { get; set; }

    public decimal PriceA { get; set; }

    public decimal PriceB { get; set; }

    public decimal PriceC { get; set; }

    public decimal SalePrice { get; set; }

    public DateTime? SaleStartDate { get; set; }

    public DateTime? SaleEndDate { get; set; }

    public int QuantityDiscountId { get; set; }

    public int TaxId { get; set; }

    public short ItemType { get; set; }

    public decimal Cost { get; set; }

    public double Quantity { get; set; }

    public double ReorderPoint { get; set; }

    public double RestockLevel { get; set; }

    public double TareWeight { get; set; }

    public int SupplierId { get; set; }

    public int TagAlongItem { get; set; }

    public double TagAlongQuantity { get; set; }

    public int ParentItem { get; set; }

    public double ParentQuantity { get; set; }

    public short BarcodeFormat { get; set; }

    public decimal PriceLowerBound { get; set; }

    public decimal PriceUpperBound { get; set; }

    public string? PictureName { get; set; }

    public DateTime? LastSold { get; set; }

    public string? SubDescription1 { get; set; }

    public string? SubDescription2 { get; set; }

    public string? SubDescription3 { get; set; }

    public string? UnitOfMeasure { get; set; }

    public int SubCategoryId { get; set; }

    public bool QuantityEntryNotAllowed { get; set; }

    public bool PriceMustBeEntered { get; set; }

    public string? BlockSalesReason { get; set; }

    public DateTime? BlockSalesAfterDate { get; set; }

    public double Weight { get; set; }

    public bool Taxable { get; set; }

    public byte[]? DbtimeStamp { get; set; }

    public DateTime? BlockSalesBeforeDate { get; set; }

    public decimal LastCost { get; set; }

    public decimal ReplacementCost { get; set; }

    public bool WebItem { get; set; }

    public int BlockSalesType { get; set; }

    public int BlockSalesScheduleId { get; set; }

    public int SaleType { get; set; }

    public int SaleScheduleId { get; set; }

    public bool Consignment { get; set; }

    public bool Inactive { get; set; }

    public DateTime? LastCounted { get; set; }

    public bool DoNotOrder { get; set; }

    public decimal Msrp { get; set; }

    public DateTime DateCreated { get; set; }

    public string? UsuallyShip { get; set; }

    public string? NumberFormat { get; set; }

    public bool? ItemCannotBeRet { get; set; }

    public bool? ItemCannotBeSold { get; set; }

    public bool? IsAutogenerated { get; set; }

    public bool IsGlobalvoucher { get; set; }

    public bool? DeleteZeroBalanceEntry { get; set; }

    public int TenderId { get; set; }

    public string? Notes { get; set; }

    public string ExtendedDescription { get; set; } = null!;

    public string Content { get; set; } = null!;
}