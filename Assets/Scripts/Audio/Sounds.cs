using UnityEngine.Audio;
using UnityEngine;

/* 
   Como pretendemos configurar os áudios diretamente no inspetor da unity,
   tornamos a classe serializável, ou seja, permitimos que essa classe, 
   tenha seus atributos expostos no inspetor.
*/
[System.Serializable]
/*
    Classe dos sons, contendo todas as informações necessárias para tocar os sons
    de acordo com as configurações desejadas para cada um deles. Caso precise de outras
    configurações, adicionar aqui.
*/
public class Sound {

    //nome do som, o áudio vai ser chamado a partir do mesmo
    public string name;

    //arquivo de som
    public AudioClip clip;

    //volume do som, onde o Range() coloca um slider no inspetor para uma configuração mais simples
    [Range(0f, 1f)]
    public float volume; 

    //pitch do som, com o Range() para o slider, novamente
    [Range(.1f, 3f)]
    public float pitch;

    //determina se o som, quando chegar ao fim, irá ou não ser tocado novamente
    public bool loop;

    //fonte de cada som, onde se esconde a informação no inspetor, já que ele é alterado automaticamente via código
    [HideInInspector]
    public AudioSource source;
}