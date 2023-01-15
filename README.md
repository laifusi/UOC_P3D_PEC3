# Práctica Final

## Cómo jugar
Para iniciar el juego será necesario acceder a la escena Menu en la carpeta Scenes del proyecto.

Al iniciar el juego y tras pulsar la opción "Play" en el menú, el jugador se encontrará en un pueblecito. En este, hay un plaga de zombies de los que deberá protegerse con su arma. En el escenario hay cinco puntos de entrada de zombies que el jugador deberá cerrar mediante cristales mágicos. Para conseguir estos cristales, tendrá que acabar con los zombies gigantes. Una vez tenga un cristal, si se acerca a una de las puertas de entrada, esta se cerrará. El juego se acaba cuando el jugador cierra las cinco puertas o cuando los zombies matan al jugador.

Para controlar al personaje, se tendrán que utilizar las teclas WASD para el movimiento y el espacio para el salto. Por otro lado, haciendo clic izquierdo con el ratón, se disparará, mientras que moviendo el ratón por la pantalla se podrá apuntar y mover la cámara.
Si el jugador se encuentra cerca de un coche, pulsando la tecla E podrá entrar en este y conducirlo por el pueblo. Para controlar el coche, se usarán las teclas WASD para el movimiento y la tecla E para salir del modo conducción.

Al iniciar el nivel es necesario disparar una vez para que se active correctamente la funcionalidad de apuntar.

Por el escenario, encontrará items de vida y de munición que le ayudarán a sobrevivir más tiempo.

Además, por el escenario irán apareciendo humanos y coches que se moverán por el espacio de forma autónoma. Los primeros herirán al personaje si choca contra ellos, mientras que los segundos se convertirán en zombies si son atrapados por otro zombie.

## Estructura e Implementación
En esta ampliación del trabajo anterior, mantenemos la división en dos escenar: el menú y el nivel.

Desde el menú, añadimos la opción "Options", que abre un panel desde el que se podrá escoger si la música y/o los sonidos están activos. Esta funcionalidad se consigue usando un script MusicManager, que mantenemos entre escenas puesto que lo usamos de AudioSource de la música; y el Audio Mixer de Unity, mediante Snapshots con los que definimos qué Mixer Group - "Music" o "Effects" - suena, en función de lo escogido por el jugador. Para indicar si está seleccionado el ON o el OFF, actualizamos el texto de los botones de opciones mediante la clase OptionsTextUI.

Dentro del nivel, encontramos varios elementos nuevos a comentar: los peatones y los coches.

En el caso de los peatones, creamos una nueva máquina de estados, reutilizando y renombrando la interfaz de los zombies a IAIState. En este caso, definimos dos estados: WalkState y RunAwayState. En el primero, el peatón se moverá de un punto a otro de forma aleatoria, hasta escoger un punto definido como "building", en el que desaparecerá. Si entra en el Trigger con el que el zombie ya detectaba al jugador en el trabajo anterior, reaccionará a haber visto al zombie en un tiempo aleatorio entre dos valores y pasará al estado de huida. En este segundo estado, calculará el "punto seguro" más cercano y correrá hacia él. Hemos definido por el espacio lo que serían los puntos seguros y se los pasamos en una lista al instanciarlos, del mismo modo lo hacemos con los "puntos de ciudad" y los "puntos de edificio". Si es atrapado, destruimos el objeto e instanciamos un zombie en su lugar. Para definir toda la funcionalidad del peatón, usamos la clase PedestrianAIController.

Para instanciar a los peatones usamos una nueva clase que hereda de la clase Spawner definida en el trabajo anterior, PedestrianSpawner. Mediante el que definimos cuando hay que instanciarlos, de forma constante en el tiempo, y el Spawn. En el método Spawn, instanciamos un peatón aleatorio de entre tres prefabs en una posición aleatoria de entre los spawn points definidos. Le pasamos, además, los puntos del espacio que podrá usar de destino al nuevo peatón.

Los coches, por su parte, tienen tres modos: estático, AI y controlado por el jugador.

Para pasar de uno a otro, creamos un CarManager que contiene los tres tipos y se encarga de activar o desactivar el coche en función de cuál deba estar activo. Además, con el método ResetTransforms recolocamos las versiones que no se han movido a la posición de la última versión usada, cuando se produce un cambio de estado.

En el caso del coche controlado por el jugador, usamos el asset de StardardAssets que permite el funcionamiento que buscamos. A este le añadimos una clase Car con la que detectamos si el jugador pulsa la tecla E para salir del coche. Al jugador, le añadimos una clase Driver que hace la función inversa, detecta si el jugador pulsa la tecla E para entrar al coche. En referencia a esto, a todos los tipos de coche les hemos añadido un trigger con el que poder detectar si el jugador quiere robar el coche y usarlo.

Para el coche autónomo, usamos el asset de Standard Assets que utiliza un circuito de waypoints para moverse por el espacio. En este caso, creamos un par de circuitos posibles, a los que les definimos un punto de inicio, donde instanciaremos el coche, y un punto de salida, donde haremos que el coche desaparezca. Estos spawns los hacemos un otro Spawner, CarSpawner, que funciona de una forma similar al PedestrianSpawner. Si ha pasado cierto tiempo desde el último coche instanciado, creamos uno nuevo, en un punto aleatorio de entre los posibles y con el circuito asignado. En este caso, lo hacemos usando una clase que hemos definido como CarLine, que simplemente guarda el punto de entrada y el circuito que le corresponde. Para el punto de salida, creamos un trigger y una clase CarExitPoint que se encarga de detectar cuando un coche de tipo IA entra en él y llama a su CarManager para que lo destruya.

A este tipo de coche, le añadimos, además, la clase CarAIFrontDetector y un trigger, con los que detectamos si delante del coche hay otro coche, un peatón o el jugador. Si es así, hacemos que el coche se pare hasta que el elemento detectado salga del trigger. Para esto, añadimos un método a la clase CarAIControl - SetDriving(bool driving) - con el que hacemos que frene o vuelva a acelerar. En la clase CarAIFrontDetector, además, si lo que se detecta es otro coche, mandamos un aviso a este indicándole que está bloqueando la carretera. Para la recepción de estos avisos, creamos una clase CarBlockDetector, que calculará durante cuanto tiempo ha estado bloqueando la carretera y destruirá el coche si hace demasiado tiempo, siempre que no esté siendo controlado por el jugador.

Finalmente, a los coches en movimiento les añadimos un trigger delante - y detrás en el caso de aquellos controlados por el jugador - con el que detectar los atropellos. Les añadimos la clase CarRunOverDetector, donde comprobamos qué tipo de objeto entra al trigger y si se trata de un zombie o de un peatón, los hacemos explotar - en los métodos GetRunOver de ZombieAIController y PedestrianAIController, respectivamente. Si, por el contrario, atropellamos al jugador, se le aplica una cantidad de daño.


## Problemas conocidos
El control del jugador mediante el ThirdPersonController se podría mejorar y no permite al jugador moverse en el salto si este movimiento no se ha iniciado anteriormente, no he encontrado la forma de solucionarlo sin dejar de usar el asset estándar.

En el escenario, la unión de alguno de los colliders de los assets provoca que el jugador se impulse hacia arriba como si tropezara con ellos, a simple vista no parece que esto deba pasar pero no he encontrado el motivo.

Al iniciar el nivel es necesario disparar una vez para que se active correctamente la funcionalidad de apuntar.

## Vídeo
Este es el [enlace](https://youtu.be/9tUgbhGmeI4) al vídeo de la PEC.
