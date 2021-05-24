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

    //banca
    public int saldo = 1000;
    public int apuesta = 0;
    public Text saldoText;
    public Text apuestaText;
    public Button apuestadiez;
    public Button apuestaCincuenta;
    public Button apuestaCien;
    public Button apuestaCincoZeroZero;
    //fin banca

    public int[] values = new int[52];
    
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();
        InitBanca();
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
            if(cont > 1 && cont <= 9) //CADA 9 cartas
            {
                values[i] = cont; //se pone los valores del 1-10
            }
            else if(cont > 9 && cont <=13) //los Q J K se ponen a 10
            {
                values[i] = 10;
            }
            else if(cont ==14 || cont == 1) //cuando aparece un as
            {
                cont = 1;
                values[i] = 11; //se pone a 11 y se reinicia el contador
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
        int casosProbables = 0;
        float probabilidad1;
        int diferencia = 0;
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador*/
        if (dealer.GetComponent<CardHand>().cards.Count >= 1) //la primera carta es al player la segunda al dealer
        {
            //puntos del dealer sin la carta tapada
            int pointsVisibleDealer = dealer.GetComponent<CardHand>().points - dealer.GetComponent<CardHand>()
                .cards[0].GetComponent<CardModel>().value; //puntos de las cartas visibles, le resto la carta oculta
            //casos en los que probables en los que el dealer tiene mas puntos que tu
            //casos probables de cartas = 13(num de palos) - (puntos + los visibles);
            casosProbables = 13 - player.GetComponent<CardHand>().points + pointsVisibleDealer; //13 tipos de cartas, 13 - la diferencia de los 2 player,dealer, para saber
            probabilidad1 = casosProbables / 13f; //casos probables / los casos posibles
            if (probabilidad1 > 1) //si supera el 100%
            {
                probabilidad1 = 1;
            }else if(probabilidad1 < 0) //si es menor que 0%
            {
                probabilidad1 = 0;
            }
            diferencia = player.GetComponent<CardHand>().points - pointsVisibleDealer; // si da mas de 11 , entoces el dealer no puede tener cartas mayores que a eso
            if (diferencia >= 11)
            {
                probabilidad1 = 0;
            }

            probabilidad1 = probabilidad1 * 100;
            probMessage.text = "Prob 1 : " + probabilidad1.ToString() + " %";
            
        }
        /* - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta*/

        // prob de llegar a 17
        int casosProbables2 = 0; //prob de llegar a 17
        float probabilidad2 = 0.0f;
        float probabilidad21 = 0.0f;
        //casos probables de cartas que supern 17 = 13(num carta) - (16 - puntosPlayer) (para saber cuantos puntos le faltan para llegar a 17)
        casosProbables2 = 13 - (16 - player.GetComponent<CardHand>().points); //casos de cartas en las que llegas a 17 puntps
        probabilidad2 = casosProbables2 / 13f;
        if (probabilidad2 > 1)
        {
            probabilidad2 = 1;
        }
        else if (probabilidad2 < 0)
        {
            probabilidad2 = 0;
        }
        if (player.GetComponent<CardHand>().points < 6) // un numero menos de 6 no puede llegar a 17 puntos o mas
        {
            probabilidad2 = 0;
        }

        


        /* - Probabilidad de que el jugador obtenga más de 21 si pide una carta*/
        int casosProbables3 = 0;
        float probabilidad3 = 0.0f;
        casosProbables3 = 13 - (21 - player.GetComponent<CardHand>().points); //los casos de las cartas en las que puedes pasarte de 21
        probabilidad3 = casosProbables3 / 13f; //casos prob / los que hay
        if (probabilidad3 > 1)
        {
            probabilidad3 = 1;
        }
        else if (probabilidad3 < 0)
        {
            probabilidad3 = 0;
        }
        if (player.GetComponent<CardHand>().points < 11) //como un numero menor de 11 no puede super los 21 , caso imposible
        {
            probabilidad3 = 0;
        }
        probabilidad3 = probabilidad3 * 100;
        probMessage.text +="\nProb 3: " + probabilidad3.ToString() + " %";


        //prob entre 17 y 21
        probabilidad21 = probabilidad2 - probabilidad3;// a las prob de llegar a 17 le restas las prob de pasarte de 21
        if (probabilidad21 > 1)
        {
            probabilidad21 = 1;
        }
        else if (probabilidad21 < 0)
        {
            probabilidad21 = 0;
        }
        probabilidad21 = probabilidad21 * 100;
        probMessage.text += "\nProb 17-21: " + probabilidad21.ToString() + " %";
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
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        hitButton.interactable = false; //descactivo boton

        while(dealer.GetComponent<CardHand>().points <= 16) // si tiene 16 puntos o menos insertar cartas
        {
            
            PushDealer(); //insertar carta
        }

        int actualDealerPoints = dealer.GetComponent<CardHand>().points; //get los points del dealer
        if (actualDealerPoints == 21) //si es blackjack
        {
            PlayerHaPerdido("El player ha perdido! BlackJack del dealer");
        }
        else if(actualDealerPoints >=17) //si tiene mas o igual a 17 puntos
        {
            if(actualDealerPoints > 21) { //si se pasa de 21 puntos
                PlayerHaGanado("El player ha ganado! dealer tiene mas de 21 puntos!");
            }
            else //sino se pasa
            {
                int actualPlayerPoints = player.GetComponent<CardHand>().points; //get los points del player
                if (actualDealerPoints.Equals(actualPlayerPoints)) //si hay empate
                {
                    Empate("Empate con " + actualPlayerPoints + " puntos los dos");
                }
                else //sino hay empate
                {
                    if(actualPlayerPoints > actualDealerPoints) //si el player tiene mas puntos
                    {
                        PlayerHaGanado("Ha ganado el Player por puntuacion mayor");
                    }
                    else //sino
                    {
                        PlayerHaPerdido("Ha ganado el dealer por puntuacion mayor");
                    }
                }

            }
        }
        
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
            Empate("Empate! BlackJack!");
        }
        else
        if (actualPlayerPoints == 21) //blackjack jugador
        {
            PlayerHaGanado("El player a ganado! BlackJack");
        }
        else
        if (actualDealerPoints == 21) //blackjack dealer
        {
            PlayerHaPerdido("El player ha perdido! BlackJack del dealer!");
        }
    }

    private void comprobarPlayer()
    {
        int actualPlayerPoints = player.GetComponent<CardHand>().points; //get los points del player
        if (actualPlayerPoints == 21) //blackjack jugador
        {
            PlayerHaGanado("El player a ganado! BlackJack");
        }
        else if (actualPlayerPoints > 21)
        {
            PlayerHaPerdido("El player a perdido!");
        }
    }


    //parte opcional banca
    private void InitBanca()
    {
        saldoText.text = saldo.ToString();
        apuestaText.text = apuesta.ToString();
    }
    public void Apuesta(string numApuesta)
    {
        int apuestaInt = int.Parse(numApuesta);  //converitr de string a int
        if (saldo >= apuestaInt)
        {
            saldo = saldo - apuestaInt; // restar la apuesta al saldo
            apuesta += apuestaInt; //sumar el saldo
            if (saldo == 0)
            {
                DesactivarBotonesBanca();
            }
            InitBanca();
        } 
    }

    private void DesactivarBotonesBanca()
    {
        apuestaCien.interactable = false;
        apuestaCincoZeroZero.interactable = false;
        apuestaCincuenta.interactable = false;
        apuestadiez.interactable = false;
    }
    private void ActivarBotonesBanca()
    {
        apuestaCien.interactable = true;
        apuestaCincoZeroZero.interactable = true;
        apuestaCincuenta.interactable = true;
        apuestadiez.interactable = true;
    }

    private void PlayerHaPerdido(string texto)
    {
        finalMessage.text = texto; //texto del ganador
        hitButton.interactable = false; //desactivo el boton
        stickButton.interactable = false;//desactivo el boton
                                         //get la carta girada del dealer
        CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
        dealerToggleFace.ToggleFace(true); //rotarla

        apuesta = 0;
        InitBanca();
    }

    private void PlayerHaGanado(string texto)
    {
        finalMessage.text = texto; //texto del ganador
        hitButton.interactable = false; //desactivo el boton
        stickButton.interactable = false;//desactivo el boton
        CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
        dealerToggleFace.ToggleFace(true); //rotarla

        int ganadorApuesta = apuesta * 2;
        saldo += ganadorApuesta;
        apuesta = 0;
        InitBanca();
        if(saldo != 0)
        {
            ActivarBotonesBanca();
        }
    }

    private void Empate(string text)
    {
        finalMessage.text = text; //texto del ganador
        hitButton.interactable = false; //desactivo el boton
        stickButton.interactable = false;//desactivo el boton
        CardModel dealerToggleFace = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>();
        dealerToggleFace.ToggleFace(true); //rotarla

        saldo += apuesta;
        apuesta = 0;
        InitBanca();
        if (saldo != 0)
        {
            ActivarBotonesBanca();
        }
    }

}
