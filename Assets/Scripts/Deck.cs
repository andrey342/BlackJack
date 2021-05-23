using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        int cont = 0 ;
        for (int i=0; i<= values.Length-1; i++)
        {   
            
            cont++;
            if(cont <= 9) //CADA 9 cartas
            {
                values[i] = cont; //se pone los valores del 1-10
            }
            else if(cont > 9 && cont <=13) //los Q J K se ponen a 10
            {
                values[i] = 10;
            }
            else if(cont ==14) //cuando aparece un as
            {
                cont = 1;
                values[i] = cont; //se pone a 1 y se reinicia el contador
            }
            
            
            
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        int count = values.Length; //get count de la lista
        while (count > 1) //hasta que el while no llegue al primer int de la lista
        {
            count--; //paso a otro item, 52-1= 51 ...
            int aleatorio = Random.Range(0, count + 1); //genero un iteM, aleatirio
            int aux = values[aleatorio]; //recojo el int del index aleatorio
            values[aleatorio] = values[count]; //guardo en el index aleatorio el valor del item que estamos ahora
            values[count] = aux; //guardo el aux en el item que estamos

            // shuffle faces
            Sprite auxFace = faces[aleatorio];
            faces[aleatorio] = faces[count];
            faces[count] = auxFace;
        }
        

        


    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
        ComprobarQuienHaGanadoAlPrincipio();



    }

    

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        comprobarPlayer();

    }

    public void Stand()
    {
        
        
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    private void ComprobarQuienHaGanadoAlPrincipio()
    {
        int actualPlayerPoints = player.GetComponent<CardHand>().points; //get los points del player
        int actualDealerPoints = dealer.GetComponent<CardHand>().points; //get los points del dealer

        if (actualPlayerPoints == 21 && actualDealerPoints == 21) //empate
        {
            finalMessage.text = "Empate! BlackJack!"; //texto del ganador
            hitButton.interactable = false; //desactivo el boton
            stickButton.interactable = false;//desactivo el boton
            //get la carta girada del dealer
            CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
            dealerToggleFace.ToggleFace(true); //rotarla
        }
        else
        if (actualPlayerPoints == 21) //blackjack jugador
        {
            finalMessage.text = "El player a ganado! BlackJack"; //texto del ganador
            hitButton.interactable = false; //desactivo el boton
            stickButton.interactable = false;//desactivo el boton
            //get la carta girada del dealer
            CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
            dealerToggleFace.ToggleFace(true); //rotarla
        }
        else
        if (actualDealerPoints == 21) //blackjack dealer
        {
            finalMessage.text = "El player ha perdido! BlackJack del dealer!"; //texto del ganador
            hitButton.interactable = false; //desactivo el boton
            stickButton.interactable = false;//desactivo el boton
            //get la carta girada del dealer
            CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
            dealerToggleFace.ToggleFace(true); //rotarla
        }
    }

    private void comprobarPlayer()
    {
        int actualPlayerPoints = player.GetComponent<CardHand>().points; //get los points del player
        if (actualPlayerPoints == 21) //blackjack jugador
        {
            finalMessage.text = "El player a ganado! BlackJack"; //texto del ganador
            hitButton.interactable = false; //desactivo el boton
            stickButton.interactable = false;//desactivo el boton
            //get la carta girada del dealer
            CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
            dealerToggleFace.ToggleFace(true); //rotarla
        }
        else if (actualPlayerPoints > 21)
        {
            finalMessage.text = "El player a perdido!"; //texto del ganador
            hitButton.interactable = false; //desactivo el boton
            stickButton.interactable = false;//desactivo el boton
            //get la carta girada del dealer
            CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
            dealerToggleFace.ToggleFace(true); //rotarla
        }
    }
    

}
