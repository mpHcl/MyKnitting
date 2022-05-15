using MyKnitting.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace MyKnitting.Services {
    public class YarnDataStore : IDataStore<Yarn> {
        readonly ObservableCollection<Yarn> yarns;
        public YarnDataStore() {
            yarns = new ObservableCollection<Yarn>() {
                new Yarn {Id = 0, Amount=20, Color = "Blue", ColorCode="#221aa2", Material="Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 1, Amount=10, Color = "Blue", ColorCode="BLUE", Material="Wool", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 2, Amount=30, Color = "Cyan",ColorCode="Cyan", Material="Cotton", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 3, Amount=10, Color = "Blue", ColorCode="#221ab1", Material="Cotton", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 4, Amount=2, Color = "Black", ColorCode="#111111", Material="Cotton+Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 5, Amount=5, Color="Purple", ColorCode="Purple", Material="Moher", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 6, Amount=6, Color="Navy blue", ColorCode="#020859", Material="Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 7, Amount=12, Color = "Aquamarine", ColorCode="Aquamarine", Material="Wool", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 8, Amount=1,  Color = "Gray",  ColorCode="#aabbcc", Material="Silk", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 9, Amount=2, Color="White", ColorCode="#FFFFFF", Material="Wool", Name="Knitting for Olive", Owned = false, Size = 4},
                new Yarn {Id = 10, Amount=2, Color ="Gray", ColorCode="#AAAAAA", Material="Cotton", Name="Knitting for Olive", Owned = false, Size = 4},
            };
        }

        public Task<bool> AddItemAsync(Yarn item) {
            yarns.Add(item);
            return Task.FromResult(true);
        }
        public Task<bool> UpdateItemAsync(Yarn item) {
            return null;
        }
        public Task<bool> DeleteItemAsync(string id) {
            yarns.Remove(yarns.Where(x => x.Id.ToString() == id).First());
            return Task.FromResult(true);
        }
        public async Task<Yarn> GetItemAsync(string id) {
            var intId = int.Parse(id);
            return await Task.FromResult(yarns.FirstOrDefault(item => item.Id == intId));
        }
        public async Task<IEnumerable<Yarn>> GetItemsAsync(bool forceRefresh = false) {
            return await Task.FromResult(yarns);
        }
    }
}
