using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using MonopolyMAUI.Server;
using MonopolyMAUI.ViewModel;
using MonopolyMAUI.Graphics;

namespace MonopolyMAUI.View;

public partial class GamePage : ContentPage/* : StartGame*/
{
    Grid mainGrid;
    Image info;
    public static GamePage Instance { get; private set; }

	public GamePage(GameViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        mainGrid = this.FindByName<Grid>("MainGrid");
        //Пример добавления цвета вниз карточки
        /*var vertical = this.FindByName<VerticalStackLayout>("Highway_Realty");
        vertical.Add(new Label()
        {
            HeightRequest = 5,
            BackgroundColor = Colors.Red,
        });*/

        Instance = this;

        var stack = this.FindByName<VerticalStackLayout>("Players_0");
        foreach(var player in PlayersList.Players)
            stack.Add(new GraphicsView()
            {  
                StyleId = $"Player_{player.UserEntity.Nickname}",
                Drawable=new PlayerPoint(player.UserEntity.Color), 
                HeightRequest=12,
            });
    }

    public async Task TempAsync(object sender, EventArgs e)
    {
     
    }

    void TappedLeftRecognizer(object sender, TappedEventArgs args)
    {
        var trigger = new Trigger(typeof(Entry))
        {
            Property = IsFocusedProperty,
            Value = true
        };
        if (info != null && mainGrid.Contains(info))
            mainGrid.Remove(info);
        
        var tapGestureRecognizer = (Image)sender;
        info = new Image
        {
            Source = ImageSource.FromFile($"{args.Parameter}.png"),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            HeightRequest = 210,
            WidthRequest = 117,
            
        };

        var gestureRecognizer = new TapGestureRecognizer
        {
            Buttons = ButtonsMask.Primary
        };

        gestureRecognizer.Tapped += (s, e) =>
        {          
            mainGrid.Remove(info);
        };

        info.GestureRecognizers.Add(gestureRecognizer);
        
        mainGrid.Add(info);
    }

    public void OnPressedImageButton(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var name = imageButton.StyleId;
        /*Можно попробовать TapGestureRecognizer у Image.GestureRecognizers и по нажатию на левую кнопку - просмотр инфы, на левую - окно покупки или продажи домов как вариант*/
        //Как получать картинки для вывода 
        //Элементы можно связывать через Uid
        //TODO сделать так, чтобы при нажатии ImageButton появлялся FlyOut для описания недвижимости
    }
    /*
     Вместо кнопок на странице использовать гриды и внутри кнопки и картинки
     */
    /*<CollectionView>
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type model:User}">
                //TODO доделать с моделькой User + поле(может в процентах)
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate x:DataType="model:User">
                <HorizontalStackLayout Padding="10">
                    <Image
                    Aspect="AspectFill"
                    HeightRequest="100"
                    Source="{Binding Image}"
                    WidthRequest="100" />
                    <VerticalStackLayout VerticalOptions="Center">
                        <Label Text="{Binding Name}" FontSize="24" TextColor="Gray"/>
                        <Label Text="{Binding Cash}" FontSize="18" TextColor="Gray"/>
                    </VerticalStackLayout>
                </HorizontalStackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>*/

    /*Игрок: <Ellipse WidthRequest="15" 
                                 HeightRequest="15" 
                                 Fill="LightGreen" 
                                 StrokeThickness="3" 
                                 Stroke="Black" 
                                 HorizontalOptions="Center" 
                                 ZIndex="10"/>
    Цвет принадлежности игроку: <Label HeightRequest="5" VerticalOptions="End" BackgroundColor="Red" ZIndex="1"/>
    Дома и отели: <Image Source="house.png" HeightRequest="12" WidthRequest="12" />
                            <Image Source="hotel.png" HeightRequest="12" WidthRequest="12" />*/

}