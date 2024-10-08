﻿namespace BobyDesignWeb.Data.Entities
{
    public enum OrderStatus
    {
        Opened = 1,
        Closed = 2
    }

    public enum OrderType
    {
        Standard = 1,
        Reclamation = 2,
    }

    public enum OrderPaymentMethod
    {
        Cash = 1,
        Card = 2
    }
}
