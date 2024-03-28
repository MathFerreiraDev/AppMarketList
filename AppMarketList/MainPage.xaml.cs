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
            await Navigation.PushAsync(new Views.NovoProduto());

            
        }

        protected async override void OnAppearing()
        {
           if(list_products.Count == 0)
            {
              
                    List<Produto> tmp = await App.Db.SelectAll();
                    foreach (Produto p in tmp)
                    {
                        list_products.Add(p);
                    }
                
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            list_products.Clear();
          
                List<Produto> tmp = await App.Db.SearchProduct(q);
                foreach (Produto p in tmp)
                {
                    list_products.Add(p);
                }
            
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

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Produto? p = e.SelectedItem as Produto;

            await Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p
            });
        }

        private async void MenuItem_Clicked_Remover(object sender, EventArgs e)
        {
            try
            {
                MenuItem selected = (MenuItem)sender;
                Produto p = selected.BindingContext as Produto;
                bool confirm = await DisplayAlert("Tem certeza?", $"Remover produto {p.Descricao}", "Confirmar", "Cancelar");

                if(confirm)
                {
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso", $"O item {p.Descricao} foi removido da lista!", "OK");
                    list_products.Remove(p);
                }
            }catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}
