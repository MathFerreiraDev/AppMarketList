using AppMarketList.Models;
using System.Collections.ObjectModel;

namespace AppMarketList
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Produto> list_products = new ObservableCollection<Produto>();

        public MainPage()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = list_products;
        }

        private void btn_Somar_Clicked(object sender, EventArgs e)
        {
            double somador = list_products.Sum(i=> (i.Preco * i.Quantidade) );
            string msg = $"Soma total em itens: {somador:C}";
            DisplayAlert("Resultado",$"A somatória de sua lista é equivalente a: {somador:C}", "Concluído");
        }

        private async void btn_Add_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//FormularioProduto");
        }

        protected override void OnAppearing()
        {
           if(list_products.Count > 0)
            {
                Task.Run(async () => {
                    List<Produto> tmp = await App.Db.SelectAll();
                    foreach (Produto p in tmp)
                    {
                        list_products.Add(p);
                    }
                });
            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            list_products.Clear();
            Task.Run(async () =>
            {
                List<Produto> tmp = await App.Db.SearchProduct(q);
                foreach (Produto p in tmp)
                {
                    list_products.Add(p);
                }
            });
        }

        private void ref_carregando_Refreshing(object sender, EventArgs e)
        {
            list_products.Clear();
            Task.Run(async () =>
            {
                List<Produto> tmp = await App.Db.SelectAll();
                foreach (Produto p in tmp)
                {
                    list_products.Add(p);
                }
            });
            ref_carregando.IsRefreshing = false;
        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void MenuItem_Clicked_Remover(object sender, EventArgs e)
        {
            try
            {
                MenuItem selected = (MenuItem)sender;
                Produto p = selected.BindingContext as Produto;
                bool confirm = await DisplayAlert("Tem certeza?", "Remover produto?", "Confimrar", "Cancelar");

                if(confirm)
                {
                    await App.Db.Delete(1);
                    await DisplayAlert("Sucesso", "O item foi removido da lista!", "OK");
                }
            }catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}
