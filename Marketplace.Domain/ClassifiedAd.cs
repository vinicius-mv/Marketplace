using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain;

public class ClassifiedAd
{
    public ClassifiedAdId Id { get; private set; }
    private UserId _ownerId;

    public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
    {
        if (id == default)
            throw new ArgumentException("Identity must be specified.", nameof(id));

        Id = id;
        _ownerId = ownerId;
    }

    public void SetTitle(string title) => _title = title;

    public void UpdateText(string text) => _text = text;

    public void UpdatePrice(decimal price) => _price = price;

    private string _title = string.Empty;
    private string _text = string.Empty;
    private decimal _price;
}
