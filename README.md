Este GDD no contiene imágenes. Para ver el GDD completo, consultar el PDF de la entrega.

Versión 0.2.8

**_ÍNDICE_**

[RESUMEN 2](#_Toc628238059)

[Concepto de juego 2](#_Toc833591514)

[Ficha descriptiva 2](#_Toc778574296)

[Experiencia de juego 3](#_Toc1824374219)

[HISTORIA 3](#_Toc1533871092)

[Trama principal 3](#_Toc1712859091)

[La Guerra 3](#_Toc234459644)

[Idria 4](#_Toc939943110)

[Los Núcleos 5](#_Toc2003947985)

[DISEÑO DE NIVELES 5](#_Toc285896660)

[Mapa de mundo 5](#_Toc652013313)

[Fase de Exploración y Combate 5](#_Toc994801342)

[Fase de Calma y Diálogo 6](#_Toc1360907887)

[Fase de Desafío 6](#_Toc1912376602)

[Fases 6](#_Toc387530304)

[Fase de Combate y Exploración 6](#_Toc1286830973)

[Fase de Calma y Diálogo 7](#_Toc2101792321)

[Fase de Desafío 8](#_Toc1989358201)

[Sala de la Gran Pira 8](#_Toc1227745595)

[MECÁNICAS 9](#_Toc1264523689)

[Mecánicas Principales 9](#_Toc1319790177)

[Vela 9](#_Toc1798986953)

[Hechizos 9](#_Toc785658574)

[Elementales 9](#_Toc1160431850)

[De forma 11](#_Toc120593802)

[Libro 11](#_Toc27412353)

[Mapa 11](#_Toc1541076372)

[Mecánicas Secundarias 12](#_Toc1975398438)

[Objetos 13](#_Toc200123561)

[Enmarcar objetos 13](#_Toc1123375023)

[Obtención de fragmentos 13](#_Toc923350107)

[Lista de objetos 13](#_Toc1980388618)

[Común 14](#_Toc2073431059)

[Raro 14](#_Toc2032471153)

[Épico 15](#_Toc1121436235)

[Legendario 16](#_Toc802981324)

[Eventos 17](#_Toc1824318996)

[Puntuaciones 17](#_Toc1642541372)

[Scoreboard 17](#_Toc1845406106)

[CONTROLES 18](#_Toc373166217)

[INTERFAZ 19](#_Toc1911663961)

[Flujo del Juego 19](#_Toc2049909938)

[Pantalla de bienvenida 19](#_Toc692137952)

[Menú principal o Menú inicio 19](#_Toc998045216)

[Diagrama de flujo 22](#_Toc1876274556)

[PERSONAJES 23](#_Toc2056240142)

[Jugador 23](#_Toc2143594762)

[Ficha de personaje 23](#_Toc453157104)

[Sprite sheet 23](#_Toc1018978873)

[Enemigos 24](#_Toc1049411558)

[Inferi 24](#_Toc1934060478)

[Descripción 24](#_Toc249289771)

[Diagrama descriptivo: Máquina de estados jerárquica y Behaviour Tree 24](#_Toc808994783)

[Interacción con el Mundo 26](#_Toc235909762)

[Murciélago gigante 27](#_Toc434043373)

[Descripción 27](#_Toc7177317)

[Diagrama descriptivo: Árbol de toma de decisiones binario 28](#_Toc2014312038)

[Interacción con el Mundo 29](#_Toc1918428548)

[Vampiro 30](#_Toc1527466808)

[Descripción 30](#_Toc1869589061)

[Diagrama descriptivo: Máquina de estados jerárquica 30](#_Toc906577275)

[Interacción con el Mundo 31](#_Toc723150661)

[Hombre de cobre 31](#_Toc1460993096)

[Descripción 31](#_Toc1329582460)

[Diagrama descriptivo: Máquina de estados jerárquica 32](#_Toc2081647416)

[Interacción con el Mundo 33](#_Toc585808750)

[Sombras 34](#_Toc1815572458)

[Descripción 34](#_Toc916347829)

[Diagrama descriptivo: Máquina de estados jerárquica 35](#_Toc1691450450)

[Interacción con el Mundo 36](#_Toc688341446)

[Aliados 37](#_Toc2005873864)

[Luciérnaga 37](#_Toc1215227866)

[Descripción 37](#_Toc782716410)

[Diagrama descriptivo: Máquina de estados 37](#_Toc691429687)

[Interacción con el Mundo 38](#_Toc1523419517)

[Aldeano 38](#_Toc2059934143)

[Descripción 38](#_Toc1282503670)

[Diagrama descriptivo: Behaviour Tree 38](#_Toc348612728)

[Interacción con el Mundo 39](#_Toc1358076720)

[ESTÉTICA VISUAL 40](#_Toc1472945186)

[Moodboard 40](#_Toc1110496331)

[Concept Art 41](#_Toc362976478)

[Arte 2D 42](#_Toc1344002422)

[Diseño de Personajes 42](#_Toc210194862)

[Personaje principal 42](#_Toc1403074340)

[Arte 3D 44](#_Toc1470739980)

[Logotipo 45](#_Toc1320781543)

[Elementos de UI 46](#_Toc1962987132)

[Fondo para el menú de inicio 46](#_Toc1682328389)

[Botones y desplegables 46](#_Toc709988483)

[Iconos 46](#_Toc1786221638)

[ESTÉTICA SONORA 46](#_Toc519375372)

[Música 47](#_Toc619239416)

[Efectos de sonido 47](#_Toc1624233944)

[VERSIONES 47](#_Toc104122257)

[Planteamiento 47](#_Toc1718900565)

[Alfa 49](#_Toc2066631623)

[Generación de Mundo 49](#_Toc2018905399)

[Mapa del Mundo 49](#_Toc402022435)

[Generación de nivel 49](#_Toc1054102856)

[Sistema de hechizos 49](#_Toc2074749151)

[Sistema de combate 49](#_Toc1263869300)

[Movimiento por el mapa 49](#_Toc377760707)

[Libro de hechizos 49](#_Toc88469750)

[Beta 49](#_Toc643906367)

[Sustituir Placeholders 49](#_Toc1381871973)

[Diseño e implementación de habitaciones 49](#_Toc1549286779)

[Añadir diálogos 49](#_Toc1629786812)

[Implementar inventario 49](#_Toc964711224)

[Comportamiento de enemigos 49](#_Toc2085436964)

[Detalles del funcionamiento de habitaciones 49](#_Toc1692647428)

[Interacción con Interfaces 49](#_Toc245650330)

[Retroalimentación visual 49](#_Toc2145689675)

[Scoreboard online 49](#_Toc462764078)

[“Mi Firmamento” 49](#_Toc1942080431)

[Añadir Bestiario 49](#_Toc493501935)

[Expandir el GDD 49](#_Toc232304178)

[Gold 49](#_Toc934618178)

[Implementar shaders y sistema de partículas 49](#_Toc263718537)

[Mejora visual 49](#_Toc1154745065)

[Añadir contenido extra 49](#_Toc347234325)

[Añadir logros y mejora de “Mi Firmamento” 49](#_Toc1795941036)

[Reglas de torneo 49](#_Toc995669372)

[Efectos de sonido y música 49](#_Toc494895342)

[Bugs resueltos y videojuego adaptado a distintos dispositivos, navegadores web y controles 49](#_Toc773401104)

[POST-MORTEM 49](#_Toc641611136)

[MODELO DE NEGOCIO 49](#_Toc1519294515)

[Público objetivo 49](#_Toc1498389998)

[Mapa de empatía 49](#_Toc2598382)

[¿Qué piensa y siente? 49](#_Toc1463805417)

[¿Qué oye? 49](#_Toc1070603469)

[¿Qué ve? 49](#_Toc1143957330)

[¿Qué dice y hace? 49](#_Toc203734813)

[Caja de herramientas 49](#_Toc440904776)

[Lienzo o Canvas 49](#_Toc1873703614)

[Socios Clave 49](#_Toc1200018565)

[Actividades Clave 49](#_Toc1772308198)

[Recursos Clave 49](#_Toc991168052)

[Propuesta de Valor 49](#_Toc1095760457)

[Relación con el Cliente 49](#_Toc729359776)

[Canales de Distribución 49](#_Toc1295039087)

[Segmentos de Clientes 49](#_Toc108661033)

[Estructura de Costes 49](#_Toc1711660209)

[Fuentes de Ingresos 49](#_Toc75725724)

[MONETIZACIÓN 49](#_Toc72574399)

[REDES SOCIALES 49](#_Toc1864426528)

[Cuenta de Velorum 49](#_Toc1208703897)

[Posts 49](#_Toc1605355994)

# RESUMEN

## Concepto de juego

Candelight es un videojuego en el que controlamos a un humano que se ha sacrificado, enlazando su alma a la llama de una vela. Nuestro objetivo es iluminar un mundo que ha sido condenado a una noche eterna, para proteger a sus habitantes de monstruos.

El protagonista usará los poderes mágicos que canaliza la vela para enfrentarse a ellos, combinando elementos y lanzando hechizos. Tendrá que encender la mayor cantidad de Grandes Piras antes de que la vela se quedé sin cera y se extinga, junto a su vida.

## Ficha descriptiva

**Nombre del juego:** Candelight

**Estudio de desarrollo:** Velorum

**Participantes:** Carlos Garrido Guerrero, Elena Milara Mencía, Adrián Mira García y Juan Alessandro Vázquez Bustos.

**Género:** Roguelike, acción en tercera persona.

**Estéticas:** Fantasía, desafío, descubrimiento, narrativa, competición y expresión.

**Estilo artístico:** Arte 2D pixelart con elementos 3D.

**Ambientación:** Mundo fantástico medieval.

## Experiencia de juego

Se busca una experiencia inmersiva y dinámica con elementos tácticos. Los escenarios principales están centrados en el combate, que cuenta con un sistema desafiante de combinación de hechizos en tiempo real.

Por otro lado, también se fomenta el factor competitivo mediante un scoreboard, que tiene una disposición creativa e inusual, integrado con la estética y narrativa del juego.

# NARRATIVA

## Trama principal

El mundo es plano. Es un disco flotante en el espacio que se sostiene sobre un núcleo de luz. Dicho núcleo está conformado por corrientes mágicas y, cuanto más cerca del núcleo, más fuertes son estas corrientes.

Con tal de sobrevivir, el núcleo se alimenta de las almas de todos aquellos que mueren, quedándose estas retenidas en las corrientes mágicas. Se trata de un proceso natural, de una simbiosis del mundo para mantener el equilibrio y así vivir eternamente. Sin embargo, si hay demasiadas muertes este equilibrio se rompe, ya que no nacen suficientes criaturas en la superficie terrestre para paliar las defunciones y el núcleo se termina quedando sin alimento.

Debido a una guerra entre los humanos que habitan la superficie, es precisamente lo que ha sucedido en Candelight. Este es un mundo en declive, que envejece y lucha por sobrevivir. Es más, durante la guerra, debido la alta tasa de muertes, emergieron cristales de propiedades mágicas de la corteza terrestre. De estos pilares mágicos brotaron nuevas criaturas, monstruos que reniegan de la luz y que encarnan los deseos del núcleo de matar a todos aquellos sobre la faz de Candelight en un intento fútil de reestablecer el equilibrio.

Para evitar que las criaturas de la noche ataquen a la humanidad, el protagonista ha tomado la decisión de alumbrar el mundo con una vela de luz del núcleo. Ha hecho un gran sacrificio, vinculando su alma a la llama que ha tomado. Esto le permite controlar las corrientes del núcleo a menor nivel, conjurar magia. Sin embargo, si el fuego de la vela se apaga, él también morirá.

## La Guerra

En Candelight existían tres civilizaciones humanas: Durnia, Temeria e Idria.

La guerra se libró entre las dos civilizaciones más relevantes: Durnia y Temeria. Ambas son ajenas a la naturaleza del núcleo, y apenas se preocupan por la integridad del mundo. Las batallas produjeron un gran número de muertes que harían escasear el número de almas y también mermaron los recursos, sumergiendo tanto a Durnia como a Temeria en la pobreza y hambruna.

Es durante la guerra que, debido a la sobrealimentación del núcleo, se generaron los pilares mágicos y la tenue luz diurna que iluminaba el mundo se extinguió, convirtiéndose en una noche eterna. Los monstruos comenzarían a emerger de los pilares en la segunda mitad del conflicto, pero no supondrán una gran amenaza hasta llegado el final de la guerra.

Temeria se encontraba cerca de la victoria, pues apenas quedaban habitantes vivos de Durnia. Pero, dadas las grandes pérdidas que habían sufrido y los pocos recursos que podrían ganar si proseguían con la contienda, optaron por firmar un tratado de paz con Durnia, la cual parecía proclive a aceptar el tratado. Sin embargo, Durnia traicionó la confianza de Temeria en el último momento, quebrantando la oferta de paz con un ataque contundente. Y es que Durnia, cerca de su derrota, descubrió el potencial de los cristales mágicos que habían emergido de la tierra. Imbuyeron objetos y armas en fragmentos extraídos de los pilares, canalizando su magia para potenciar los objetos y así otorgarles propiedades mágicas. Con esta estrategia, Temeria sucumbió al ataque de Durnia.

Desde ese día, los habitantes de Durnia se autoproclamaron, “El Alba”, un nuevo nombre que ensalza su victoria y unidad, presagiando un supuesto nuevo amanecer. En la actualidad, apenas treinta años tras la guerra, son una élite fragmentada, sus escasos supervivientes esparcidos por el mundo. Siguen teniendo acceso a la tecnología de los fragmentos, guardándose para sí todos los conocimientos y perfeccionando sus artilugios.

Por otro lado, Temeria quedó en la ruina tras la derrota. A pesar de ser con diferencia la población más numerosa en Candelight, luchan por mantenerse en pie. Hoy día, siguen sin saber cómo hacer uso de los fragmentos, tratando los objetos que llegaron a sus manos como meros residuos de la guerra. Los habitantes, condenados a vivir en la nueva noche eterna y sin la ayuda de la magia para defenderse, son asediados constantemente por los monstruos.

## Idria

Es una civilización que se cree extinta. Y los habitantes de Candelight desconocen el motivo de su desaparición. Incluso antes de la guerra, mantenían poco contacto con Idria, atribuyéndole un halo de misterio.

Los idrianos siempre han tratado de mantenerse al margen de conflictos, centrándose en cuidar y conocer mejor el mundo en el que viven. Se recluyen en lugares ignotos, actuando de forma pacífica y neutral frente al resto de civilizaciones. Cuando estalló la guerra, decidieron no involucrarse en ella, pues temían que esta provocase un desbalance en el mundo.

Una vez los monstruos emergieron de los pilares, muchos de los idrianos tomaron la iniciativa de sacrificarse al núcleo para portar su luz. De esta forma, quizás pudiesen expandirla y frenar el ansia del núcleo el tiempo suficiente como para que la humanidad resurgiese. Sin embargo, todos acabaron pereciendo en el intento y se convirtieron en unos monstruos de entrañas de cera que deambulan sin alma por el mundo, los inferi.

El motivo de su desaparición es incierto. Aunque se sospecha que fueron una combinación de factores los que les condujeron a la extinción: la amenaza de los monstruos, los sacrificios realizados para llevar la llama y los daños colaterales de la guerra. Además, existe el rumor de que se involucraron en peligrosos experimentos relacionados con la naturaleza del núcleo.

## Los Núcleos

La realidad del universo es que todos los astros son núcleos semi conscientes, de idéntica naturaleza al de Candelight. Tan solo se diferencian en su edad, que se ve afectada por su estabilidad. Si un núcleo se mantiene estable, sería eterno. Pero eso nunca sucede, siempre llega un punto de inflexión en donde se rompe el equilibrio.

Los soles son núcleos infantes, recién nacidos del universo y las supernovas son aquellos que mueren antes de poder crear un mundo del que alimentarse, antes de alcanzar la madurez.

Un mundo sano, adulto, es aquel que dispone de una superficie con vida, la suficiente para brillar y sobrevivir sin eclipsarla.

Es cuando no hay suficientes almas que el ansia del núcleo aumenta. Y en los picos de escasez o abundancia de alimento es cuando el núcleo genera los cristales, envolviendo en oscuridad el mundo, entrando en declive. Y en ese afán de alargar todo lo posible su vida, crean criaturas de los cristales que se encargan de asesinar a todo ser vivo.

Sin almas de las que alimentarse, el núcleo se encierra entre los filamentos de fragmentos, quedando rodeado de corteza inerte, muerto. Los planetas son cadáveres.

Y toda vida que nazca sobre la superficie de un cadáver no conocerá la magia.

## Tramas secundarias

Existen otros elementos narrativos secundarios a los que se les hace mención en diálogos, eventos y descripciones de localizaciones y objetos.

Algunos ejemplos son los cuentos y leyendas populares que están arraigados en las culturas de Temeria y el Alba; el misterio sobre la aparición de flores azules en zonas quemadas; información sobre lugares recónditos de Candelight, como las Cataratas del Borde o el Fondo; detalles sobre los avistamientos de monstruos...

También se aportan diferentes perspectivas sobre acontecimientos históricos, anécdotas y experiencias de cada aldeano.

Toda referencia a las tramas adicionales y pistas que desvelan poco a poco la historia principal, se encuentran embebidas en los diálogos, eventos y descripciones de localizaciones.

He aquí el enlace al documento complementario en el que quedan recopilados:

[https://urjc.sharepoint.com/:w:/r/sites/gr\_2024.2175\_2175038\_AT-VELLORUM/\_layouts/15/Doc2.aspx?action=editNew&sourcedoc=%7B7190a88a-8e25-4cf1-83be-9c69e29b0934%7D&wdOrigin=TEAMS-MAGLEV.teamsSdk\_ns.rwc&wdExp=TEAMS-TREATMENT&wdhostclicktime=1732462566642&web=1](https://urjc.sharepoint.com/:w:/r/sites/gr_2024.2175_2175038_AT-VELLORUM/_layouts/15/Doc2.aspx?action=editNew&sourcedoc=%7B7190a88a-8e25-4cf1-83be-9c69e29b0934%7D&wdOrigin=TEAMS-MAGLEV.teamsSdk_ns.rwc&wdExp=TEAMS-TREATMENT&wdhostclicktime=1732462566642&web=1)

# DISEÑO DE NIVELES

## Tutorial

Si es la primera vez que el jugador se adentra en el mundo de Candelight, se le introduce sus mecánicas clave mediante un tutorial.

### Primera parte

La primera parte del tutorial está enfocada en brindar al jugador un primer contacto con la narrativa del juego. Además, se la cámara se encuentra en primera persona, pues la intención es lograr una mayor inmersión del jugador.

El jugador aparecerá en una plataforma oscura desde donde, al comienzo, solo verá pilares blancos en la lejanía. Puede moverse, caminar hacia adelante. Y conforme avanza, verá que la plataforma tiene un final. Al asomarse, podrá ver el Núcleo.

No tendrá otra opción que tirarse, sacrificarse al Núcleo. Y, al tomar esa decisión, se escuchará una música de tensión mientras se precipita a la enorme esfera de magma.

Una vez caiga en ella, hay un fundido a blanco para pasar a la siguiente parte del tutorial.

**Segunda parte**

En medio de la nada se mostrarán al jugador mediante texto los controles que necesita aprender para conjurar hechizos.

La sucesión de pasos a realizar es:

\- Pulsar tecla B.

\- Mantener pulsado Espacio y después soltarlo.

\- Mantener pulsado Shift y después soltarlo.

\- Por último, mantener pulsado Espacio y se mostrará una sucesión de flechas que se corresponden a las teclas AWSD.

### Tercera parte

Se desarrolla en un entorno de estética 2D y la cámara pasa a estar en tercera persona. Es la perspectiva que se tomará en la mayor parte del juego.

La tercera y última parte del tutorial se divide en diferentes pruebas que se realizan en una especie de limbo, un espacio controlado para que el jugador se acostumbre a las mecánicas.

#### 1ª Prueba

#### Se bloquean las salidas y se muestra un mural en el que aparece una combinación de flechas. Para completar la prueba, el jugador debe abrir su página de anotaciones con la tecla B y, mientras presiona Shift o Espacio, anotar despacio la combinación que se indica. Así aprenderá su primer hechizo de forma, el Proyectil.

Para invocarlo se debe mantener la tecla Shift, que va asociada a los hechizos de Forma, y realizar mientras tanto la combinación de flechas con AWSD. El proyectil debe impactar contra los obstáculos para romperlos y así pasar a la siguiente prueba.

Para dejar de mirar la página, hay que volver a pulsar la tecla B.

#### 2ª Prueba

La segunda prueba consiste en disparar a dos obstáculos que están conectados a uno más grande. Para acertarles, hay que invocar el proyectil de nuevo y apuntar con la flecha del ratón en la dirección en la que quieras lanzarlo.

Una vez el jugador destruye ambos obstáculos pequeños, el grande desaparece y se puede continuar.

#### 3ª Prueba

El jugador se enfrenta a un enemigo común del videojuego: el Murciélago Gigante. Para derrotarle, tendrá que hacer uso de los conocimientos que ha aprendido.

Una vez el jugador le derrota, podrá recolectar sus fragmentos e irse. Sin embargo, en el camino de salida se le dejará una pista al jugador para que aprenda un nuevo elemento.

Si lo registra, se le desvela que es capaz de combinar más de un elemento a la vez. Para ello, tendrá que incluir ambas recetas de elementos en la combinación de flechas, sin importar el orden.

Cuando llegue al final del camino, tendrá que pulsar E para usar la Antorcha de Salida.

## Mapa de mundo

Una vez empieza la partida, el jugador aparece en el mapa del mudo. Se encuentra situado encima de un nodo, que representa el primer nivel que debe completar.

Cada nodo cuenta con de una a cuatro fases, distintos tipos de escenarios con una funcionalidad única:

### Fase de Exploración y Combate

Centrada en enfrentarse a enemigos y avanzar por una mazmorra.

### Fase de Calma y Diálogo

Donde se puede interactuar con aldeanos y recuperar parte de la vida.

### Fase de Desafío

Una sala donde se lucha contra oleadas de enemigos.

El orden en el que aparecen las fases, así como el número de veces que pueda generase ese tipo de fase en un nivel, es aleatorio.

Cuando el jugador finaliza todas las fases de un nivel, pasa a una escena especial que siempre se encuentra al final del nivel, la **Sala de la Gran Pira**, donde debe encender una enorme antorcha central para regresar al mapa del mundo.

En el mapa del mundo, figurará que el nivel en el que se encuentra está completado, quedando iluminado.

Una vez completado el nivel, el jugador podrá desplazarse por el mapa mediante un sistema de carriles hacia los nodos adyacentes al nivel o niveles completados.

Dentro del apartado de Mecánicas, se detalla en mayor profundidad el funcionamiento de los nodos y el desplazamiento por el mapa.

## Fases

### Fase de Combate y Exploración

Se trata de la más fase común de todas, apareciendo con más frecuencia que las demás fases, y en la que gira la mayor parte del gameplay del juego.

Consiste en una mazmorra compuesta por habitaciones, que se generan de forma procedural, así como los pasillos que sirven de conexión entre ellas. El número de habitaciones es aleatorio dentro de un rango, así como el tipo al que pertenecen.

Los tipos de habitaciones se clasifican en:

**Sala de Combate**

Tiene enemigos dentro, que atacarán al jugador.

Una vez el jugador entra en la sala de Combate, se materializan unos obstáculos o puertas en las entradas para que no pueda huir fácilmente y así motivarle a plantarle cara a los enemigos. Estos obstáculos se pueden destruir con hechizos, pero requieren esfuerzo.

Una vez el jugador ha acabado con todos los enemigos de la sala, los obstáculos que le bloqueaban las salidas desaparecen.

**Sala de Pista**

Alberga una pista integrada en el entorno que desvela una nueva combinación que permita al jugador desbloquear un nuevo hechizo. Este tipo de sala se genera con muy poca frecuencia, y no puede haber más de una por fase De Combate y Exploración.

**Sala Vacía**

Sirven para confundir al jugador sobre la posible existencia de una pista y, al no haber enemigos, hace más ameno y dinámico el avance del jugador por la mazmorra.

**Sala de Evento**

Se trata de una habitación que se genera en escasas ocasiones, donde el jugador puede tomar una decisión interactuando con el entorno. Los eventos son variados y pueden afectar a fases posteriores del nivel.

El completar eventos fomenta que los aldeanos otorguen objetos en fases posteriores de Calma y Diálogo de ese mismo nivel.

**Sala con Antorcha de Salida**

Es una sala donde hay una pequeña antorcha que, al ser iluminada por el jugador, le teletransporta a la siguiente fase del nivel, permitiéndole salir de esa mazmorra.

### Fase de Calma y Diálogo

Es una pequeña mazmorra conformada por muy pocas habitaciones. En ninguna de ellas habrá enemigos.

Tiene distintos tipos de salas, y siempre aparecerá una sala de cada tipo, aunque puede haber más de una Sala de diálogo.

**Sala de Diálogo**

Contiene aldeanos con los que el jugador puede interactuar.

Al hacerlo, se abre un diálogo, donde el aldeano le puede desvelar información sobre el mundo al jugador, dar pistas sobre cómo enfrentarse a enemigos e incluso, con menos frecuencia, le proporcionará un objeto al jugador. El objeto pasará automáticamente al inventario, pero encontrará desactivado.

El diálogo es la única forma de conseguir objetos en el juego.

**Sala de Curación**

Es una pequeña sala con un Altar de Curación en el medio. Si el jugador interactúa con él, tiene la opción de intercambiar fragmentos a cambio de recuperar parte de la vida, regenerando así la cera que tiene la vela.

**Sala con Antorcha de Salida**

En la Fase de Calma y Diálogo también habrá una Sala con Antorcha de Salida, que le permitirá al jugador abandonar la fase actual.

### Fase de Desafío

Se trata de una única sala, bastante espaciosa, donde se generarán sucesivamente, con un intervalo de descanso entremedias, una serie de oleadas de enemigos. El número de oleadas y su dificultad variará según la dificultad del nivel.

Hasta que el jugador no acabe con todos los enemigos, no comenzará la siguiente oleada.

Una vez completado el desafío, se le otorgará al jugador como mínimo un fragmento garantizado y aparecerá una pequeña Antorcha de Salida en el medio de la sala.

## Sala de la Gran Pira

Es una escena especial, a la que siempre se llevará al jugador una vez haya completado todas las fases del nivel.

Se trata de una única sala, en donde hay una enorme pira apagada en el medio. El jugador podrá encenderla interactuando con ella, marcando la completitud del nivel.

Una luz blanca se expandirá desde la Gran Pira hasta ocupar toda la pantalla, creando una transición en donde se teletransportará al jugador al mapa del mundo. El nivel, ahora completado, brillará.

# MECÁNICAS

El jugador deberá desplazarse por un conjunto de pueblos con el fin de ayudarlos y devolverles la luz antes de que se acabe la suya propia. Aquí se desarrollan en detalle las mecánicas que conforman la jugabilidad del juego:

## Mecánicas Principales

### Vela

La vela representa la vida del jugador, los niveles que podrá recorrer hasta perder la luz para siempre y morir.

La cantidad de cera disminuye por cada nivel que completa el jugador, así como por los golpes que reciba de los enemigos. La luz se apagará cuando la cera se agote y la partida acabará.

El cuánta cera quede se muestra visualmente en el HUD, simulando una barra de vida vertical.

Puede conseguirse más cera en Altares de Curación, que se encuentran en la Fase de Calma y Diálogo de los niveles. Requieren que el jugador canjee fragmentos a cambio de recuperar cera de la vela.

### Hechizos

El jugador puede combinar glifos a tiempo real, representados con flechas. Para ello, debe seguir un patrón concreto para ejecutar el hechizo a su elección. Los hechizos interactúan con los enemigos y con algunos elementos del entorno.

Las combinaciones de glifos sirven a modo de receta para invocar los hechizos. Y, para que funcionen, será preciso tenerlas registradas en el Libro. La única excepción son aquellos hechizos que vienen aprendidos por defecto.

Dependiendo de lo poderoso que se un hechizo, dispondrá de una receta más o menos complicada. Es decir, los hechizos más complejos tienen recetas con mayor número de glifos y con patrones más complejos.

Las recetas de los hechizos están enlazadas con la semilla del mundo; si cambia la semilla, cambia toda la lista de recetas de forma aleatoria. Además, las recetas desbloqueadas se conservan de una partida a otra, siempre que se mantenga la semilla.

Los hechizos se dividen en dos categorías:

#### Elementales

Se trata del tipo base que poseen los ataques del mago. El establecer elemento no lanza un ataque, sino que indica qué clase de poder elemental se va a usar a la hora de lanzar ataques, las características que va a tener.

Siempre y cuando disponga de la receta de los elementos suficientes, el jugador puede seleccionar uno o dos elementos. Hay un objeto concreto que aumenta este máximo de elementos a tres.

Si se tiene un único elemento equipado, el daño y características vendrán determinados por dicho elemento. Sin embargo, teniendo equipados dos elementos, se hará una media del daño que hacen los dos elementos cuando se lancen ataques. En cuanto a las características de los dos elementos escogidos, se aplicarán ambas.

Los elementos esquipados se muestran en el HUD y dichos elementos se mantendrán equipados hasta que el jugador los cambie con una nueva combinación de uno o dos elementos.

Los elementos son:

**Fuego** (por defecto)  
Con el impacto de sus ataques deja quemado al enemigo. Hace ligero daño repetidas veces hasta que el efecto desaparece a los pocos segundos. Además, interactúa con la madera y la hierba, quemándolas.

**Electricidad**  
Los ataques de tipo electricidad paralizan momentáneamente al enemigo cuando impactan contra él. Además, interactúa con el agua, electrizándola; y puede activar máquinas (solo en eventos).

**Cósmico**  
Los proyectiles de tipo cósmico atraen a enemigos hacia donde impactan. Por otro lado, tanto la explosión cósmica como el ataque cuerpo a cuerpo repelen a enemigos. La diferencia es que el ataque cuerpo a cuerpo repele a un enemigo en concreto mientras que la explosión afecta a todos los enemigos en un área de alrededor del jugador.  
Además, los ataques de tipo cósmico sobrecargan objetos mágicos, haciéndolos que exploten y que repelan y dañen a los enemigos. E impulsa objetos (solo en eventos).

**Fantasmal**  
Todos los ataques de tipo fantasmal ralentizan al enemigo. Es más, los proyectiles se autodirigen al enemigo. La explosión fantasmal consiste en generar proyectiles fantasmales alrededor del jugador, que se autodirigen a los enemigos más cercanos.

Además, el elemento fantasmal tiene la capacidad de calmar a seres vivos (solo en eventos).

#### De forma

Activan el ataque o efecto al momento. Están sujetos a las características del elemento o elementos equipados.

Los hechizos de forma son:

**Cuerpo a cuerpo** (por defecto)  
Ejecuta un ataque adyacente al jugador en la dirección en la que esté mirando.

**Proyectil**  
Invoca un proyectil desde la posición del jugador que se dispara en la dirección en la que esté apuntando.

**Explosión**  
Genera un ataque en área en torno al jugador.

**Potenciación**  
Aviva el fuego de la vela, provocando que los ataques del elemento o elementos equipados causen más daño.

### Libro

Es una herramienta que permite al jugador registrar nuevas recetas de hechizos y así poder ejecutarlos.

Al equipar el libro, el jugador puede introducir los glifos, las flechas, que conformen un nuevo hechizo. Si acierta, el libro marcará qué hechizo ha aprendido con un símbolo.

Las recetas de los hechizos registrados se mostrarán en pantalla a la hora de querer invocarlos, para que el jugador no sienta la necesidad de memorizárselas.

Las nuevas combinaciones de hechizos se pueden encontrar en pistas dentro de la Fase de Combate y Exploración, que vendrán integradas en el entorno dentro de las Salas Pista.

Es más, cabe la posibilidad de que el jugador acierte de casualidad introduciendo instrucciones aleatorias en el libro. Sin embargo, se dificultará gracias a un delay por cada intento de receta introducida en el libro.

### Mapa

Se genera un mapa aleatorio acorde a la semilla, en forma de grafo. El número de nodos es aleatorio dentro de un rango, así como las conexiones de nodos adyacentes entre ellos Como mínimo, cada nodo debe tener una conexión, para que se accesible.

Cada nodo representa un nivel. El jugador podrá moverse mediante carriles a los nodos adyacentes a nodos completados. Para ello, primero debe indicar el carril al que se quiere dirigir, con los controles de movimiento; esto hace rotar una flecha de dirección en torno al jugador que señaliza el carril escogido. Para confirmar su selección, el jugador pulsa la tecla de espacio, lo que le hará desplazarse hacia el nodo indicado.

Los niveles completados aparecerán remarcados, brillando, para simbolizar que ya se ha iluminado esa zona del mapa. Y los niveles a los que se pueda acceder, se conectarán con caminos que sirven a modo de carriles. Por otro lado, los niveles que no son accesibles por el jugador, pero que son visibles en pantalla, se ocultarán parcialmente con una niebla de guerra.

Existen distintos tipos de niveles, catalogados por el bioma al que pertenezcan. Son tres los tipos de biomas, cada uno centrado en una civilización distinta: Durnia (“El Alba”), Temeria e Idria. La proporción de niveles de un bioma concreto puede variar en base a la semilla. Dependiendo del bioma al que pertenezca el nivel, el nodo tendrá un color diferente y la estética dentro del nivel variará.

Cada bioma representa a su vez la dificultad del nivel, que se mostrará cuando el jugador se encuentre situado encima del nodo del nivel. El bioma más común es el de dificultad Intermedia, representado por Temeria; el siguiente más común es el de Durnia, de dificultad Fácil; y, por último, el que será más escaso es el bioma de dificultad Difícil, representado por Idria.

La dificultad de un nivel afecta a cuántos enemigos aparecen en las Fases de Combate y Exploración y en las Fases de Desafío, así como a los tipos de enemigos que se generen, el número de oleadas en las Fases de Desafío, la aparición de objetos de mayor rareza conforme aumente la dificultad y la escasez de Fases de Calma y Diálogo.

Cada nivel contiene de una a cuatro fases, las cuáles pueden ser de varios tipos. Los tipos de fases que pueden aparecer son de: Exploración y Combate, Calma y Diálogo y Desafío. La disposición de nodos y el número de fases que contenga cada uno se queda albergado en la semilla. Una vez el jugador acaba todas las fases de un nivel, el accederá a la Sala de la Gran Pira para marcar como completado el nivel y salir de vuelta al mapa de mundo.

Los niveles completados quedan marcados y se desbloqueará el acceso a niveles adyacentes. Es más, los nodos iluminados y caminos se conservan de una partida a otra, siempre y cuando se mantenga la semilla. Es más, la puntuación, que viene dada por la cantidad de niveles que ha completado el jugador, se guarda entre partidas, con la única posibilidad de aumentar.

Por último, cabe mencionar que, en cuanto a narrativa del juego, las historias contadas en los niveles serán cortas e independientes, favoreciendo así su rol como piezas que repartir aleatoriamente por el mapa. Girarán alrededor de eventos concretos o sobre el trasfondo del mundo.

## Mecánicas Secundarias

### Objetos

Los objetos son habilidades pasivas y solo se pueden obtener mediante diálogos concretos.

Se obtienen desactivados, aunque estén guardados en el inventario, no se aplican sus propiedades hasta que se activen. Para activarlos, se requiere de un número determinado de fragmentos.

Están categorizados por rareza. Las rarezas determinan no solo aproximan el poder del objeto, sino que, sobre todo, especifican el número fijo de fragmentos necesario para activarlo y que así se apliquen sus propiedades.

Así como los objetos pueden ser activados desde el inventario canjeando los fragmentos, también se pueden desactivar para recuperar los fragmentos gastados.

Hay objetos que son únicos, solo se genera uno en todo el mapa; objetos con un máximo de veces que pueden aparecer en la semilla y, por último, objetos sin un número máximo de apariciones. Es por ello, que se pueden tener objetos repetidos y acumular sus efectos al tenerlos activados.

### Enmarcar objetos

Los objetos se pierden al morir; sin embargo, desde el inventario, existe la opción de “enmarcar” un único objeto, lo que hace que ese objeto se conserve tras la muerte y se pueda usar en la siguiente partida. Si al morir el objeto enmarcado estaba desactivado, en la siguiente partida también estará desactivado; por el contrario, si estaba activado, ya aparecerá activado en el inventario en la siguiente partida. Si hay un objeto repetido, se conservará todo el grupo de ese mismo objeto al enmarcarlo.

Los marcos se pueden quitar, y el jugador podrá usarlos para proteger otros objetos.

Aunque solo se pueda enmarcar un objeto por defecto, existe un objeto que permite tener marcos adicionales y así salvaguardar más objetos. Al inicio de la partida, se enmarcará el primer objeto que consiga el jugador. Y figurarán en el inventario el número de los marcos adicionales que pueda tener, listos para aplicarse sobre los objetos que el jugador desee enmarcar.

### Obtención de fragmentos

Los enemigos tienen una probabilidad de soltar un fragmento al morir. Dependiendo de la clase de enemigo que sea, puede tener mayor probabilidad de soltar un fragmento. Además, el completar una Fase de Desafío, otorga un fragmento al jugador.

Los fragmentos se conservan durante la partida. Existe un balance, a nivel de diseño de juego, entre el número de fragmentos que se consiguen, frente al número de fragmentos requeridos para activar cada objeto. Los fragmentos también sirven como moneda de cambio para regenerar parcialmente vida en los Altares de Curación.

Los fragmentos no se conservan entre partidas.

### Lista de objetos

Cada objeto posee una propiedad única y tiene una rareza asignada. He aquí los objetos existentes:

#### Común

**Polvo de estrella fugaz**:  
Aumenta un 5% la velocidad.

“A cada mota de polvo estelar se le ha adherido un pequeño fragmento. ¡Qué paciencia!”

**Candil invisible**:  
La vela se agota un 5% menos cada vez que se pasa un nodo.

“Como el viento es invisible, lo mejor para frenarlo es algo que también sea invisible”.

**Venda roja**:  
Si la vela está a un cuarto de la vida, todos los ataques del jugador hacen un 5% más de daño.

“Siempre se ha usado una venda en la cabeza para concentrarse. Te estruja las ideas”.

**Zapatos etéreos** (Único):  
Si la vela tiene más de la mitad vida, la velocidad del jugador aumenta un 10%.

“La gente no se extraña porque te vea correr descalzo, sino porque vas más rápido”.

**Emblema del Alba** (Máx 3):  
Si el jugador mata a 15 enemigos en el plazo de 10 segundos, su siguiente ataque hará más daño. Mejóralo para que el plazo de matanza se amplíe a 15 segundos y, tras una última mejora, a 20 segundos. Cada vez que se complete el requisito, el tiempo se reinicia.

“Acabar con los hijos de la noche traerá al mundo un nuevo amanecer”.

**Zarpa de fuego**:  
El impacto de los ataques de fuego hace un 5% más de daño.

“Se la quitaron a un gato chamuscado. Éste tenía flores azules por todo su cuerpo y sus garras seguían en llamas.”

**Fierro de valentía** (Único):  
Los proyectiles de fuego dejan un charco de fuego. (Se ajusta a su rango de impacto; por ejemplo, si se usa Bomba de Pólvora el proyectil dejará un charco más grande al impactar.)

“Antes se usaba para marcar ganado. Ahora, se usa para marcar a los valientes.”

**Tumba para hormigas**:  
Aumenta el número de fantasmas que emergen de la explosión fantasmal.

“Incluso las criaturas más pequeñas tienen alma y merecen todo nuestro respeto”.

#### Raro

**Fuelle diminuto**:  
Los potenciadores aumentan un 10% más el daño de todos los hechizos, pero la vela se agota un 10% más rápido.

“Tan pequeño que se tiene que usar solo con dos dedos. Dicen que en una ocasión, apagó un incendio entero”.

**Muñeca de temerio (Máx 3)**:  
Los enemigos se mueven un 10% más rápido y hacen un 10% más de daño, pero aumenta ligeramente la probabilidad de que suelten cristales.

“El único consuelo en la guerra”.

**Luciérnaga:**  
Sigue al jugador a una pequeña distancia. Se acercará al enemigo más cercano para pegarse a él. Si el jugador ataca a ese enemigo, le hará el doble de daño.

Si ya hay una luciérnaga francotirador posada en el enemigo al que va, irá a otro que tampoco sea objetivo de otra luciérnaga. Si no hay ninguno disponible, seguirá revoloteando detrás del jugador.

“El mejor compañero en una noche eterna”.

**Bomba de pólvora** (Máx 3):

Los proyectiles de fuego estallan en una explosión al impactar contra un enemigo, causando daño en área. Cuantas más bombas de pólvora se lleven activadas, mayor será el área.

“Recogimos lo que nos lanzaron, lo combinamos con fragmentos y se lo devolvimos”.

**Silbato de la Muerte**:  
El potenciador fantasmal incrementa en un 5% la velocidad de los proyectiles fantasmales.

“Piensa que esto es lo que escucharán tus enemigos justo antes de morir”.

**Espejo astral**:  
Se lanza un proyectil cósmico extra detrás del otro.

“Es muy parecido a un lago que se encuentra en el borde del mundo. Refleja el firmamento”.

**Corazón de cobre** (Máx 5):  
Aumenta en un 15% el daño por impacto de los ataques de electricidad. Pero reduce un 15% el daño por impacto de los ataques de fuego.

“En algún lugar, un hombre de cobre desea tener corazón”.

#### Épico

**Mariposa de cera**:  
Revolotea alrededor del jugador. Evita que el jugador muera y le regenera un 25% la vida. Desaparece al salvarle.

“Salió de la boca de un inferi, lo vi con mis propios ojos.”

**Marco Eterno** (Máximo 3):  
Cuando está activado, proporciona un marco adicional, que servirá para conservar un objeto tras la muerte. El Marco Eterno está enmarcado por defecto y no se puede quitar su marco. Si el Marco Eterno se desactiva, el marco del objeto en el que se haya usado su marco adicional se eliminará.

Si el Marco Eterno se vuelve a activar, el jugador tendrá que seleccionar de nuevo manualmente el objeto que desea proteger con ese marco extra.

“Se dice que las obras enmarcadas se vuelven más relevantes tras la muerte”.

**Huellas de fuego** (Único):  
Los proyectiles de fuego dejan un rastro incendiario al lanzarse.

“El animal que las ha grabado se encuentra en el lado opuesto”.

**Mano de inferi** (Máximo 2):  
Todos los ataques fantasmales ralentizan un 25% más al enemigo.

“El abrazo de la muerte adormece los sentidos, pero no calmará tu alma”.

**Uña negra**:  
El rango de absorción de los proyectiles cósmicos aumenta un 10%. Y los ataques de tipo cósmico cuerpo a cuerpo y de explosión repelen un 5 % más al enemigo.

“Pertenecía a alguien capaz de atraer con la palabra y repeler con sus acciones.”

**Tormenta en frasco** (Único):  
El potenciador de ataques eléctricos siempre está activado. Pero la velocidad del jugador se reduce un 10%.

“Un frasco es más seguro que una bolsa”.

#### Legendario

**Tridente arcano** (Único):  
Se lanzan tres proyectiles en vez de uno, en abanico.

“La única prueba fehaciente de que nuestros antepasados en algún momento vivieron en las cascadas del fin del mundo”.

**Esfera de protección del Alba** (Único):  
Recibes un 15% menos de daño de ataques enemigos.

“Las armaduras están reservadas para los miembros del Alba. En su defecto, puedes tener esta esfera.”

**Ojos de búho** (Único):  
Otorga una visión especial, más información al jugador. Por ejemplo, puede ver cuántas fases tiene un nodo, cuánta vida tienen los enemigos, cuánta vida le queda a la vela...

“Una visión especial, que va más allá de lo trasmundano”.

**Vial Idriano** (Único):  
Se pueden combinar hasta tres elementos distintos en vez de dos.

“Desde luego, hay muchas cosas que aún desconocemos de los idrianos. Empezando por este extraño artefacto.”

**Reloj cósmico** (Único):  
Al tener equipado el elemento cósmico, el tiempo se detiene completamente mientras se invocan hechizos o elementos en vez de tan solo ralentizarse.

“Aunque lo parezca, el tiempo realmente no se detiene. La calma que desprende el reloj tan solo te hace pensar más rápido”.

**Dedos eléctricos** (Único):  
Todos los ataques de electricidad se propagan a enemigos cercanos. Pero la vela se consume un 5% más rápido.

“Pertenecieron al primer héroe del Alba. Sus esfuerzos no serán en vano”.

### Eventos

Los eventos aparecen con poca frecuencia, en la Fase de Combate y Exploración. El evento tiene una sala en la mazmorra reservada para ello y solo puede aparecer como máximo una Sala de Evento por fase.

Son sucesos que involucran personajes y elementos narrativos y que hacen tomar al jugador una decisión, la cual afecta a Fases de Calma y Diálogo posteriores. Por ejemplo, si el personaje ayuda a un aldeano que se encuentra atrapado, éste podría garantizarle un objeto en la Fase de Calma y Diálogo cuando vuelva a hablar con él.

Los eventos son de carácter variado y no son de obligado cumplimiento, el jugador puede optar por ignorar el evento, pero esto quizás involucre que no obtenga una posible recompensa en el futuro.

Dependiendo de cómo actúe el jugador, se puede tanto completar un evento, como ignorarlo o fallarlo. Además, algunos eventos cuentan con un desenlace alternativo.

### Puntuaciones

Al terminar definitivamente la partida, al jugador se le mostrarán en pantalla sus estadísticas y resultados. La información que se refleja será principalmente cuántos niveles ha completado, iluminado, en la partida y cuántos lleva acumulados entre todas las partidas. También pueden aparecer otras estadísticas, como fragmentos conseguidos, monstruos eliminados, la lista de objetos conseguidos, todas las recetas desbloqueadas, eventos completados, el tiempo en partida...

### Scoreboard

Tras la pantalla de puntuaciones propias; al jugador, tan solo si está conectado a internet, se le mostrará un efecto en el que se aleja la cámara del mapa, redirigiéndole a una pantalla con un lienzo negro; o estrellado, si ya se han guardado las partidas de otros jugadores.

En ese lienzo el jugador verá cómo aparece de pronto una estrella en una posición aleatoria del fondo. La estrella representa la partida del jugador, siendo su tamaño un indicador proporcional de la cantidad de puntuación que ha conseguido. La estrella y su posición en el firmamento se guardará en el servidor, para que perdure de cara a futuras partidas.

El jugador solo dispondrá de una única estrella en el firmamento, la cual podrá hacerse más grande con cada partida, dependiendo de cuánto aumente su puntuación, ya que se guarda y acumula el progreso de cada partida.

También se muestran las estrellas de todos los demás jugadores. Si se pasa el ratón por encima de una estrella concreta, se mostrará el nombre del dueño de la estrella, así como su puntuación.

Todos los jugadores juegan en la misma semilla y ésta se cambia cada semana, reiniciando el firmamento y eliminando todas las estrellas con sus respectivas puntuaciones. Si se cliquea la estrella, se ve el mundillo en miniatura que han iluminado.

El firmamento funciona a modo de comparativa entre jugadores, para fomentar la competitividad por obtener la estrella de mayor tamaño, así como la cooperación para plagar de estrellas el cielo nocturno.

# CONTROLES

| CONTROLES |
| --- |
|  | Teclado | Mando | Dispositivos táctiles |
| Moverse hacia delante | W | Joystick izquierdo | Joystick virtual en la esquina inferior izquierda |
| Moverse hacia atrás | S | Joystick izquierdo | Joystick virtual en la esquina inferior izquierda |
| Moverse hacia la derecha | D | Joystick izquierdo | Joystick virtual en la esquina inferior izquierda |
| Moverse hacia la izquierda | A | Joystick izquierdo | Joystick virtual en la esquina inferior izquierda |
| Interactuar | E | (por determinar) | Pulsar botón emergente sobre el elemento con el que se interactúa |
| Desplazarse hacia el nodo seleccionado en el mapa del mundo / Entrar modo Elementos | Espacio | (por determinar) | (por determinar) |
| Entrar modo Hechizos de Forma | Shift | (por determinar) | (por determinar) |
| Combinación para elegir un elemento a activar | Ctrl + W/A/S/D | Mantener L1 | (por determinar) |
| Combinación para hacer un hechizo de forma | Shift + W/A/S/D | Mantener L2 | (por determinar) |
| Apuntar en una dirección | Posición del ratón | (por determinar) | Gatillo virtual en la esquina inferior izquierda |
| Abrir el libro de hechizos para registrar hechizos | F | (por determinar) | (por determinar) |
| Abrir el inventario | I | (por determinar) | (por determinar) |
| Atajo de salida / Activar Pausa / Desactivar Pausa | ESC | (por determinar) | (por determinar) |

# INTERFAZ

## Flujo del Juego

### Pantalla de bienvenida

Se mostrará una cinemática en la que aparecerá el logotipo de la empresa desarrolladora del videojuego, Velorum, con transiciones de fade in (aparición progresiva) y fade out (desvanecimiento) a negro.

(meter imagen de la pantalla de bienvenida)

### Menú principal o Menú inicio

Se tratará de una pantalla con una imagen de fondo con estilo Pixel Art 2D, el logotipo del juego en la esquina superior izquierda y una serie de botones en la esquina inferior derecha. Dichos botones estarán diferenciados con relación a su tamaño. Los siguientes botones poseerán un tamaño alargado y mostrarán el nombre de la acción en texto:

**Jugar**:  
Comienza una partida, llevando al jugador a la pantalla de mapa del mundo. Siempre y cuando juegue en la misma semilla, conservará tanto los niveles que ya haya completado, así como los objetos “enmarcados” y las recetas de hechizos que previamente haya desbloqueado. La semilla del mundo también marca cuál es el nodo de nivel sobre el que aparece.

Dentro del juego, se muestra una interfaz a modo de HUD (Heads Up Display) que contiene información como el nivel de vida que le resta al jugador (en forma de la cantidad de cera que le queda a la vela), el elemento o elementos que se tienen seleccionados (en forma de orbe de hechizos debajo de la vela), las combinaciones que se han descubierto (en un pergamino), los movimientos que se están realizando, el número de fragmentos que el jugador posee cuando encuentra nuevos, el minimapa y accesos directos al inventario y al libro de hechizos.

Otra interfaz que aparece en el modo de juego es la del inventario. Esta se divide en 4 secciones: objetos no activados, objetos activados, descripción del objeto seleccionado y marcos eternos. Cada objeto muestra el número de fragmentos necesarios para activar dicho objeto y la rareza de este. Los ítems arrastrados a un marco eterno se pueden eliminar con el icono de X que se sitúa sobre estos.

**Scoreboard**:  
Redirige a una pantalla donde se muestra a cada jugador como una estrella en un firmamento. El tamaño e intensidad de la estrella de un jugador es un indicador de su puntuación en el juego. También existirá un botón en la esquina inferior derecha con la que se podrá visualizar el scoreboard como la típica tabla con nombre de usuario y puntuación.

**Créditos**:  
Es una pantalla en la que aparecerán los nombres de los miembros del equipo desarrollador con sus roles principales.

Además, al lado de estos botones aparecerán otros 2 botones de aspecto cuadrado y con un icono en vez de texto. Son los siguientes:

**Donaciones**:  
Será un botón con un icono de dólar. Al pulsar en él, los jugadores serán llevados a una pasarela de pago en la que podrán donar dinero al equipo de desarrollo.

**Ajustes**:  
Es un botón con icono de engranaje. Redirige a una pantalla con varias opciones que el jugador podrá modificar a su elección para mejorar la experiencia de uso. Tales opciones incluirán la modificación de los controles, ajustes de audio y video, afinaciones de gameplay y preferencias de accesibilidad.

Al hacer clic en una opción de las descritas, se llevará al jugador a la pantalla de juego o a una pantalla secundaria. El flujo del juego se trata de forma más detallada en el diagrama de flujo, que se explica más adelante.

## Diagrama de flujo

# PERSONAJES

## Jugador

### Ficha de personaje

**Nombre**: “Sacrificado” (Desconocido)

**Edad**: Desconocido

**Género**: Desconocido

**Contexto del personaje**:

Al principio era humano. Cuando las criaturas de la noche comenzaron a acabar con todos los humanos que quedaban sobre la faz del mundo, después de que una fatal guerra matara a la mayor parte de la población, decidió utilizar la magia del núcleo para transformarse en un ser capaz de combatir el yugo de los monstruos.

De esa forma, su vida y alma quedaban atadas al destino de una vela mágica. Esta vela es el recipiente de una llama que puede manipular para generar hechizos. Si la llama llegara a apagarse, él también moriría con ella.

**Descripción física**:

El personaje principal es un ser de luz. Su cabeza tiene la forma característica de una estrella y su cuerpo es un contenedor de luz que fluye por él en perpetuidad. En lugar de piernas, tiene un tornado de luz que lo mantiene erguido. No posee rostro, lo único visible son sus ojos amarillos.

### Sprite sheet

## Enemigos

Los enemigos tratarán de atacar al jugador para consumir la cera de la vela y así extinguir su llama. Se trata de criaturas que prefieren habitar en la completa oscuridad. Cada enemigo tiene su propio comportamiento y características.

Los enemigos son:

### Inferi

#### Descripción

Los inferi tienen ojos de cera. Y sus entrañas también son de cera.

Atacan cuerpo a cuerpo. Si pasan demasiado tiempo cerca del jugador y, tan solo si el jugador tiene equipado el elemento de fuego, el cuerpo de los inferi se derrite (disminuyendo la velocidad de ataque) y pierden vida hasta morir. Atacan hasta la muerte, pero cuánto más derretidos estén, más lento atacarán.

Los hechizos de fuego también provocan que se vayan derritiendo, así que son especialmente eficaces contra ellos.

Tienden a agruparse en manadas para atacar (uno de los inferi es líder de la manada), sobre todo si están lejos del jugador. Ya de cerca, cada inferi ataca sin fijarse si sigue en grupo.

#### Diagrama descriptivo: Máquina de estados jerárquica y Behaviour Tree

#### Interacción con el Mundo

### Murciélago gigante

### Sprite sheet

#### Descripción

Los murciélagos gigantes caminan a cuatro patas y están ciegos. Si el jugador se queda quieto y no realiza ningún ataque, no pueden localizarlo y se quedan parados.

Atacan cuerpo a cuerpo y, si ya están atacando, te localizarán mientras estés cerca, aunque no hagas ruido.

Usar el elemento fantasmal evita que oigan al jugador, y no pueden localizarlo. Mirarán confundidos a su alrededor frente a los sonidos fantasmales.

#### Diagrama descriptivo: Árbol de toma de decisiones binario

#### Interacción con el Mundo

### Vampiro

#### Descripción

El vampiro tratará de mantener la distancia con el jugador. En ese estado, el vampiro tiene la probabilidad de invocar a murciélagos gigantes cada cierto tiempo. (Quizás los murciélagos gigantes son sus mascotas, o realmente son los propios vampiros transformados.)

Si el jugador le ataca con proyectiles de frente, se cubrirá con su capa, evitando el daño del hechizo o reduciéndolo considerablemente.

Pero si el jugador está de espaldas a un vampiro, el vampiro enloquecerá y avanzará rápidamente hacia él para atacar cuerpo a cuerpo. Mientras está enloquecido, no podrá ni cubrirse con la capa ni invocar a murciélagos gigantes.

Si pasa un tiempo sin que el jugador le dé la espalda, volverá a tranquilizarse.

#### Diagrama descriptivo: Máquina de estados jerárquica

#### Interacción con el Mundo

### Hombre de cobre

#### Descripción

Los “hombres de cobre” tienen dos estados distintos: “enfadado”, donde atacan a distancia y rápidamente. En ese estado tratarán de mantener una distancia con el jugador para poder atacarle sin ponerse en riesgo.

Si se les ataca con electricidad, cambiarán a su otro estado, “calmado”, atacando de forma mansa y cuerpo a cuerpo y no a distancia.

Si pasa un tiempo sin que les dé con un ataque de electricidad, volverán a estar enfadados.

Al morir, estallan en una explosión, haciendo daño al jugador si está demasiado cerca.

#### Diagrama descriptivo: Máquina de estados jerárquicas

#### Interacción con el Mundo

### Sombras

### Sprite sheet

#### Descripción

Las sombras son ágiles y se mueven rápidamente, orbitando en círculos alrededor del jugador. Pueden atravesar paredes.

El círculo de sombras se irá empequeñeciendo con el tiempo y, si se reduce hasta tocar al jugador, éste sufrirá daño por cada sombra que haya. Y, con ello, las sombras morirán.

Se multiplicarán si no se les ataca, creando poco a poco otro anillo de sombras exterior. Las sombras tan solo pueden crear dos anillos adicionales de sombras y éstos no se regeneran.

Si el jugador tiene equipado el elemento de fuego, se alejarán, agrandando el círculo y dejando de orbitar. A continuación, cada una de las sombras dispara un proyectil hacia el jugador. Seguirán disparando cada cierto tiempo, siempre que el jugador tenga equipado el elemento de fuego.

Si reciben daño fantasmal, las sombras no perderán vida, al contrario, todas las sombras aumentarán su vida y crecerán de tamaño.

El elemento cósmico puede resultar muy útil para enfrentarse a ellas, ya que las aleja y agrupa.

#### Diagrama descriptivo: Máquina de estados jerárquica

#### Interacción con el Mundo

## Aliados

### Luciérnaga

#### Descripción

Sigue al jugador a una pequeña distancia, revoloteando detrás de él. Se acercará al enemigo más cercano para pegarse a él. Si el jugador ataca a ese enemigo, le hará el doble de daño.

Si ya hay una luciérnaga francotirador posada en el enemigo al que va, irá a otro que tampoco sea objetivo de otra luciérnaga. Si no hay ninguno disponible, seguirá revoloteando detrás del jugador.

#### Diagrama descriptivo: Máquina de estados

#### Interacción con el Mundo

### Aldeano

#### Descripción

Los aldeanos son habitantes de Candelight que se encuentran deambulando con sus quehaceres en salas en fase de calma, en donde no hay enemigos. A veces se sorprenderán al ver al jugador, ya que su apariencia ha cambiado al haberse sacrificado voluntariamente al núcleo para expandir la llama.

Si el jugador se acerca lo suficiente a un aldeano, se le dará la opción de interactuar con él para entablar diálogo. Los diálogos suelen proporcionar al jugador sobre el mundo, eventos que hayan sucedido en alguna fase de combate y exploración anterior, consejos para derrotar enemigos y, en ocasiones, se le otorgará al jugador un objeto.

#### Estética de cada una de las civilizaciones

**Del Alba (Drunia)**

Ataviados con armaduras hechas de fragmentos, que suelen ser de un color azul oscuro. Llevan el emblema del Alba en la armadura, ya sea incrustado o grabado en el cristal.

Solo se ponen casco para el combate y para festejar victorias recientes. Eso incluye el celebrar la caza antes de comer.

Su apellido es siempre “del Alba”.

**Temerios (Temeria)**

Aldeanos con vestimenta pobre, clásica del tercer estamento de nuestra Edad Media. Están vestidos con harapos y trapos sucios, que tienden a estar rotos o descosidos. Suelen estar desaliñados, con el pelo enmarañado.

A menudo llevan herramientas de cultivo en mano.

**Idrianos (Idria)**

Llevan ropajes de diferentes tipos de telas, las cuáles se entrelazan con pequeñas enredaderas. Predomina en ellas el color verde y marrón. Tienden a llevar capa, que por fuera es de color plateado y por dentro es oscura, con estrellas brillantes, simulando que están arropados por el firmamento. En sus vestimentas también reflejan su relación con el núcleo, decorándolas con patrones circulares.

Adornan su cara y piel con plantas y flores pegadas. Sus ojos son verdes.

#### Diagrama descriptivo: Behaviour Tree

#### Interacción con el Mundo

# ESTÉTICA VISUAL

Para el aspecto visual del juego se ha elegido la estética Píxel Art 2D. La paleta de colores escogida se basa en la gama de los morados y amarillos, colores que son complementarios entre sí. Esto garantiza que su combinación creará una apariencia agradable y cohesiva para el usuario.

Algunos ejemplos de juegos que siguen una estética similar son Lucky Luna (Netflix) y Virtuaverse:

## Moodboard

## Concept Art

En este apartado se presentan una selección de ilustraciones de arte conceptual creadas durante el desarrollo del videojuego. Estas imágenes capturan la visión artística y los elementos estéticos establecidos. También aparecen imágenes realizadas de ciertos elementos descartados durante su diseño.

En el apartado de Enemigos se pueden encontrar concept art de los enemigos del videojuego. Esas imágenes no están en este apartado hasta que se desarrolle el PixelArt (su apariencia en el gameplay) de cada uno de ellos.

Esta imagen muestra los diferentes modelos pensados en un principio para el enemigo “Sombra”.

Esta imagen muestra los diferentes colores de muestra elegidos para desarrollar el enemigo “Sombra”.

Esta imagen se trata del diseño antiguo del enemigo ”Hombre de Cobre”.

## Arte 2D

### Diseño de Personajes

#### Personaje principal

**Siluetas**:  
Para comenzar el diseño del personaje principal se realizaron las siguientes siluetas, de las cuales se eligió una para desarrollar el concept art a color:

**Color**:  
Finalmente se decantó por la primera silueta y se continuó con el desarrollo a color:

**Turnaround**:  
Con el desarrollo a color de referencia se prosiguió con el turnaround:

**Movimiento**:

**Animación de caminar**:

Spritesheet de la animación de desplazamiento del jugador. Debido a que carece de piernas, se ha animado el tornado de luz en su lugar.

**Animación de ataque cuerpo a cuerpo**:

Spritesheet de la animación de ataque melé (o cuerpo a cuerpo) del personaje principal.

**Animación de levitar (IDLE)**:

Spritesheet de la animación del personaje cuando no camina ni ataca.

**Animación de muerte**:

Spritesheet de la animación del personaje principal cuando es derrotado.

**Herramientas y accesorios**:

**Vela mágica**:  
El personaje principal siempre tiene en su poder una vela mágica con la que crea los hechizos para acabar con los enemigos.

#### Enemigo: Sombras

**Dividirse**:  
Spritesheet de la animación de las sombras cuando van a multiplicarse y a rodear al jugador:

**Atacar**:  
Spritesheet de la animación de las sombras cuando van a atacar al jugador:

#### Enemigo: Murciélago gigante

**Moverse**:  
Spritesheet de la animación del murciélago gigante para desplazarse:

**Atacar**:  
Spritesheet de la animación del murciélago gigante para atacar al jugador:

#### Aldeano: Drunia

**Moverse**:  
Spritesheet de la animación del aldeano para desplazarse:

**Sorprenderse**:  
Spritesheet de la animación del aldeano al sorprenderse cuando interactúa con el jugador:

**Imágenes en Diálogos**:  
Estas son las distintas imágenes introducidas para la representación de los aldeanos de Drunia en los posibles diálogos dentro del juego:

#### Aldeano: Temeria

**Moverse**:  
Spritesheet de la animación del aldeano para desplazarse:

**Sorprenderse**:  
Spritesheet de la animación del aldeano al sorprenderse cuando interactúa con el jugador:

**Imágenes en Diálogos**:  
Estas son las distintas imágenes introducidas para la representación de los aldeanos de Temeria en los posibles diálogos dentro del juego:

#### Aldeano: Idria

**Moverse**:  
Spritesheet de la animación del aldeano para desplazarse:

**Sorprenderse**:  
Spritesheet de la animación del aldeano al sorprenderse cuando interactúa con el jugador:

**Imágenes en Diálogos**:  
Estas son las distintas imágenes introducidas para la representación de los aldeanos de Idria en los posibles diálogos dentro del juego:

## Arte 3D

El arte 3D incorporada en el juego desempeña el papel tanto de guiar como se sorprender al jugador. Y eso es porque la mayor parte del juego es en 2D, por lo que los elementos en 3D quedan resaltados e impactan al jugador, saliéndose de las normas establecidas, pero, a su vez, encajando con la estética del videojuego.

En primer lugar, cabe destacar que la primera escena del tutorial del juego se ha realizado enteramente en 3D, usando figuras sencillas creadas en Unity y shaders personalizados.

El resto del juego sigue una estética 2D, a excepción de elementos importantes en el nivel con los que se puede interactuar y decoraciones que señalizan la presencia de un evento.

He aquí los modelos 3D clave empleados en el juego:

**Altar de Curación**

**Antorcha de Salida**

**Gran Pira**

Y este es el modelo 3D usado para un evento concreto:

**Bola de preso**

## Estética de los niveles

### Bioma del Alba

Se encuentra dominado principalmente por prados y, en ocasiones, por bosques, a la intemperie de la naturaleza. La Fase de Desafío se desarrolla en un claro.

En la Fase de Calma y Diálogo habrá de una a dos personas del Alba en una zona más despejada de vegetación o en un prado. En el campamento tan solo hay tiendas de tela improvisadas. Dejan sus avanzados artilugios mágicos expuestos, sin mayor protección que su mirada.

### Bioma de Temeria

Tanto la Fase de Exploración y Combate como la Fase de Desafío se desarrollan en mazmorras. Son laberínticas y oscuras, tan solo iluminadas por los cristales mágicos y líquenes brillantes. Las mazmorras son de piedra y, como en un pasado pertenecían a Idria, a veces se encuentra en sus paredes el emblema de la antigua civilización.

Por otro lado, la Fase de Calma y Diálogo se desarrolla en aldeas subterráneas. Las aldeas tienen el suelo cubierto de barro y paja, surcado por caminos de arena. Las casas son de paredes de barro y paja con tablones de madera. Las aldeas están rodeadas por campos de cultivo y sencillas vallas de madera.

### Bioma de Idria

La Fase de Exploración y Combate se sitúa en ciudadelas de altos muros de piedra piedra abandonadas. La arquitectura de las casas está mucho más desarrollada que las de Temeria, el suelo está pavimentado con piedras.

La Fase de Desafío se desarrolla o en una plaza o en una planicie de tierra.

### Elementos adicionales

Las zonas quemadas por la guerra se encuentran cubiertas de flores azules brillantes.

## Logotipo

Aquí se muestra el logotipo creado para el juego Candelight:

## Elementos de UI

### Fondo para el menú de inicio

### Botones y desplegables

### Iconos

### Elementos de la partida

### Elementos del inventario

# ESTÉTICA SONORA

## Música

La música buscará un sonido orquestal neoclásico (producido digitalmente mediante MIDIs), combinado con sintetizadores y sonidos ambientales. Para ser acorde al gameplay, será dinámica (cambiando entre misterio, acción, drama, etc). Sin embargo, durante buena parte del tiempo el juego permanecerá en silencio, limitándose a invocar canciones en momentos concretos.

Cada bioma tiene una composición y elección de instrumentos distinta, enfatizando la distinción entre ellos.

**El Alba**: La composición de su tema principal se basa en su deseo de alcanzar lo más alto pero sin poder escapar la tragedia de su pasado: a pesar de haber salido victoriosos en la guerra, ha sido a través de incontables muertes tanto aliadas como enemigas y tras una fachada de orgullo yace la tristeza. La melodía principal asciende por una tonalidad mayor, intentando animar a su gente, pero siempre termina descendiendo de nuevo. La canción toma un giro trágico en su clímax, la segunda mitad.

**Temeria**: La instrumentación del tema principal es sencilla en cuanto a que únicamente suena una guitarra clásica. Los temerios se han visto obligados a huir a las antiguas catacumbas de Candelight y la progresión de acordes hace referencia a su tragedia. A pesar de sus condiciones intentan salir adelante, ayudándose los unos a los otros, y eso representa la segunda mitad de la canción, cuando progresivamente se unen más guitarras a tocar.

**Idria**: El tema principal busca enfatizar la soledad de las calles abandonadas, la melancólica tristeza de aquellos idrianos que todavía permanecen con vida. Cabe destacar que los temas de exploración y combate de este bioma son el tema principal pero invertido y modificado, haciendo un guiño a los misteriosos motivos de la desolación de Idria y el destino de sus habitantes.

## Efectos de sonido

Se generan sonido aleatoriamente durante la partida para aumentar la inmersión del jugador en el mundo de Candelight, dependientes del bioma en el que se encuentra.

# VERSIONES

## Planteamiento

En la fase de planteamiento de juego se construyó la idea inicial del videojuego, su ambientación, género, temática y objetivo del jugador. Se partió de la frase “El jugador debe llevar consigo una vela y evitar que esta se apague”; y en base a ello se idearon las mecánicas principales, una noción aproximada.

Además, se creó simultáneamente el modelo de negocio y en qué consiste su monetización.

Una vez establecida la base, se ideó toda la trama y lore del mundo en el que se desarrolla el videojuego. Esto permitió dotar al juego de una ambientación concreta y de estructura.

El diseñar la historia a detalle también permitió ajustar el diseño del mapa a las necesidades del videojuego, integrándolo con la narrativa. He aquí un boceto a papel del mapa:

Y estas son imágenes de las hojas con las ideas para la historia y world-building:

Posteriormente, se diseñaron todas las mecánicas primarias restantes y también las mecánicas secundarias. En base a los tipos de hechizos que iba a haber, se diseñaron los enemigos, teniendo en cuenta que la dificultad estuviese balanceada y se compaginasen los poderes del jugador. No se optó por la típica receta de simplemente hacer enemigos más débiles al daño de un hechizo u otro, sino que se meditaron ventajas y desventajas alternativas.

Hubo ideas que se descartaron y otras que se mantuvieron y desarrollaron o modificaron. He aquí un boceto inicial de los hechizos y otra del inventario con los objetos:

## Alfa

Una vez planteado el videojuego, se programaron las mecánicas base, entre las que se incluyen la generación de mundo, la vida de la Vela, el Libro y los hechizos. Las mecánicas más complejas de programar han sido la generación de mundo y el sistema de hechizos.

### Generación de Mundo

La generación de mundo está ligada a una semilla, y se divide en dos apartados: el grafo del mapa del mundo y la generación de las habitaciones del nivel.

#### Mapa del Mundo

El mapa del mundo se trata de un sistema de nodos, un grafo. Y las interconexiones entre ellos son aleatorias.

Además, mediante un mapa de ruido, se ha diseñado la agrupación y distribución de biomas.

#### Generación de nivel

Consiste en la generación aleatoria de habitaciones, que pueden ser de distinto tipo. Las conexiones entre ellas también son aleatorias; es decir, todo el escenario está generado de forma procedural y sigue unos parámetros, como el rango del número de habitaciones y la cantidad de habitaciones de cada tipo que se pueden generar.

Además, la Sala de Salida está garantizado que aparezca a una considerable distancia de número de habitaciones respecto a la sala en la que inicia el jugador.

### Sistema de hechizos

Se han dividido también a nivel de programación los hechizos en dos categorías: Elementales de y De Forma. Ambas categorías disponen de sus clases abstractas, tanto como de características globales de los hechizos como de la generación de las recetas.

Es más, los hechizos elementales se han dividido a su vez en clases hijas que representan los modificadores de cada elemento.

En cuanto a las recetas, se han programado de tal forma que se generen aleatoriamente con cada semilla, siguiendo una serie de reglas, como que haya hechizos con combinaciones de mayor complejidad y más largas. Se ha garantizado que las recetas de hechizos no se repitan, para el juego no tenga problemas en vincularlas a su hechizo correspondiente.

### Sistema de combate

Un sistema sólido de hechizos permite el desarrollo del sistema de combate, el cual, por un lado, se encuentra muy avanzado de cara a los hechizos. Los modificadores se aplican correctamente y las recetas ejecutan cada hechizo y elementos como es esperado.

Por otro lado, el desarrollo de enemigos aún se haya en una fase temprana y no se dispone para el alfa de los tipos de enemigos ni de su comportamiento, que se reservará para la beta, donde se realizarán los ajustes de balanceo del combate una vez se tengan los dos pilares: hechizos y enemigos.

Asimismo, hay otro factor que involucra el combate, que son los objetos. Ya se encuentra programada su estructura y funcionamiento, y se han programado varios de los objetos concretos. Sin embargo, el sistema de inventario y fragmentos se planea implementar en la versión beta del videojuego, por lo que aún los objetos no se muestran en acción.

### Movimiento por el mapa

Una vez establecida la generación del mundo, la mecánica del desplazamiento por el mapa ha quedado bastante avanzada para la versión beta. Se puede distinguir al jugador desplazándose entre los nodos, así como los distintos tipos de nivel. Y también el acceso a cada uno de ellos funciona correctamente.

He aquí una imagen de la visualización actual del mapa:

### Libro de hechizos

Se ha implementado una animación en donde el jugador saca el Libro y la cámara pasa a estar en primera persona. Quedan registradas las recetas cuya combinación sea correcta. Por otro lado, los glifos; es decir, las flechas, sí que se visualizan conforme se escriben en el libro.

Es más, dentro de las Salas de Pista ya se puede encontrar un mural, que representa una nueva receta para poder introducirla en el Libro. En la versión Golden, la pista estará integrada y más escondida en el escenario, para que suponga un reto al jugador encontrarla y descifrarla.

## Beta

Se han realizado numerosos cambios para la versión Beta del videojuego, así como se ha añadido una gran cantidad de contenido.

### Sustituir Placeholders

Las figuras y elementos sencillos visuales que se encuentran presentes en la versión alfa se han sustituido por sus diseños artísticos finales.

Cada elemento visual en pantalla, objeto, hechizo, enemigo y personaje tiene su propio sprite asignado. Y, si cuentan con animaciones, disponen de su spritesheet correspondiente.

### Diseño e implementación de habitaciones

Se ha creado una amplia gama de habitaciones prefabricadas para los escenarios. En cada una de ellas, se distribuyen los npcs (personajes no jugadores) necesarios, dependiendo del tipo de sala y la fase a la que pertenecen.

Las habitaciones disponen de una estética coherente y estarán decoradas con sprites y, en ocasiones, modelos 3D que señalizan elementos de mayor importancia. Además, algunos elementos decorativos tienen elementos luminosos que aportan a la ambientación y visión del jugador.

### Añadir diálogos y descripciones

Se ha implementado la interfaz de diálogo de los aldeanos, con el contenido de textos correspondientes, que proporcionan información, objetos y consejos al jugador. Se cuenta además con diferentes iconos de los personajes a modo ilustrativo.

En total, actualmente en el juego hay: 316 diálogos, 30 descripciones de localizaciones y 21 descripciones de objetos.

### Implementar inventario

Se ha hecho completamente funcional el sistema de inventario, dando la posibilidad al jugador de activar y desactivar objetos canjeando sus fragmentos y visualizando correctamente toda la información necesaria de cada objeto. También existe una notificación para avisar al jugador de cuándo se ha añadido un objeto al inventario.

En total hay: 21 objetos, que han sido programados e incluidos en el juego.

### Tutorial

Se ha implementado un tutorial para facilitar el aprendizaje al jugador. De esta forma, se familiarizará con los controles y mecánicas más fundamentales, todas las necesarias para que su experiencia en el juego sea completa.

### Comportamiento de enemigos

Se distinguen los tipos de enemigos, cada uno tiene un aspecto y características distintas, así como también se ha programado un comportamiento propio.

También se ha incluido que tengan una probabilidad de soltar fragmentos al morir.

### Detalles del funcionamiento de habitaciones

En las Salas de Combate se ha añadido el sistema de obstáculos, que aparecen una vez el jugador entra a la sala, para que le cueste escapar.

También se dispone de las Antorchas de Salida en cada una de las fases, para navegar a la siguiente.

Dentro de la Sala de Calma y Diálogo, se encuentra el Altar de Curación plenamente funcional.

Además, se ha configurado el comportamiento de las Fases de Desafío, el cuántas oleadas hay y cuánto duran y que, al finalizar el desafío, se proporcione un fragmento al jugador.

### Interacción con Interfaces

Las interfaces diseñadas han pasado a ser implementadas en el videojuego, mostrando sus elementos visuales y permitiendo al jugador que interactúe con todas las opciones que se le ofrecen.

Entre las interfaces implementadas se encuentra: el inventario, el HUD (Head-Up Display), la información en el mapa del mundo, la interfaz de diálogo y los menús externos a la partida.

### Retroalimentación visual

Junto a las interfaces se aplican efectos visuales que sirvan para proporcionar retroalimentación al jugador sobre lo que está sucediendo en pantalla, en el juego.

Algunos ejemplos de dichos efectos vuales son: mostrar cuándo se registra correctamente un hechizo en el Libro y de qué hechizo se trata; iconos de los hechizos ejecutados en el momento, la vela derritiéndose que representa cuándo se pierde vida; cuándo se activa, desactiva o enmarca un objeto; cuándo un aldeano le da al jugador una recompensa; que figure el momento donde se recolecta un fragmento...

### Modelos 3D

Se han incluido modelos 3D para señalizar los elementos más importantes del nivel, y con los que además se puede interactuar. Se les ha aplicado el filtro de pixel art para que se integren bien en el juego.

### Implementar shaders y sistema de partículas

A nivel visual, se ha hecho una notoria mejora estética mediante el uso de shaders y la creación de un sistema sencillo y optimizado de partículas para que no afecte al rendimiento del juego en la página web.

Se han incluido además iluminación en los proyectiles y cada uno de ellos cuenta con su esfera de movimiento visual personalizado al detalle. También se ha añadido un efecto de luz al conjurar elementos, fundidos a rojo, negro o blanco para mostrar feedback de lo que sucede en el juego.

### Scoreboard online

Se ha creado una página web que es soportada por un servidor de Firebase con el objetivo de guardar las puntuaciones de todos los jugadores. Las puntuaciones se registran y muestran con diferentes tamaños de estrellas según su cantidad de puntos.

### Música

Ahora el videojuego cuenta con varias canciones originales de carácter profesional, compuestas por un miembro del equipo. Aportan una gran inmersión y riqueza al juego.

En total hay: 7 canciones. Y cada una cuenta con sus propias características.

### Expandir el GDD

Dado que el GDD es un documento vivo, se ha actualizado para la versión beta. No solo se ha añadido y modificado información sobre los apartados ya existentes, sino que se han añadido nuevas secciones como la del tutorial, las descripciones de personajes o un apartado para la estética de los escenarios de cada bioma.

También se han refinado los apartados ya redactados, aportando por ejemplo descripciones visuales de los enemigos e interfaces.

## Gold

### Mejora visual general

Se hará un repaso a todos los elementos visuales: modelado de escenarios, sprites de entidades y ornamentos, interfaces...

### Retroalimentación visual mejorada

Se añadirán efectos visuales que proporcionen información más precisa al jugador sobre lo que está pasando en el juego.

### Incorporación de anuncios y donaciones

Se simulará de forma lo más cercana a la realidad la incorporación de anuncios y colaboración con una ONG.

### Añadir el resto de los enemigos

Se incorporará al juego los enemigos que faltan por implementar.

### Añadir contenido extra

Incorporación de más variedad de descripciones de localizaciones, salas prefabricadas, sprites nuevos de aldeanos... Todo, con el objetivo de enriquecer el juego.

### Enmarcar objetos

Se añadirá la mecánica de “enmarcar” objetos para salvaguardarlos y que perduren para las próximas partidas en esa semilla.

### “Mi Firmamento”

Se añadirá un nuevo apartado en el menú principal del juego llamado “Mi Firmamento”. Si se accede a él, se mostrará al jugador un canvas vacío, un lienzo negro. Funcionará de forma muy similar al scoreboard online; sin embargo, cuenta con unas diferencias fundamentales.

Mi Firmamento dispondrá en un borde el número de estrellas acumuladas sin colocar. Cada estrella representa las partidas del jugador en una semilla concreta. Para conseguir más, el jugador deberá intentar jugar todas las semanas que pueda.

Este firmamento es propio, exclusivo del jugador y no se mostrarán las puntuaciones ni estrellas de otros usuarios. Su función es mostrar al jugador su progreso.

Además, dejará paso a la expresión y creatividad del jugador, ya que, a diferencia del scoreboard, el jugador podrá escoger dónde aparece cada una de sus estrellas y recolocarlas a su antojo. Esto podría hacer que haga dibujos únicos, personalizando el canvas a su manera.

### Añadir Bestiario

Para fomentar la sensación de coleccionismo entre los jugadores, se añadirá una nueva interfaz, un apartado en el que se visualice el progreso del jugador respecto a qué elementos se ha encontrado en las partidas. Se tratará de un libro, un Bestiario que marcará la apariencia y descripciones de los objetos y enemigos que el jugador se haya encontrado en la totalidad de sus partidas

### Añadir logros y mejora de “Mi Firmamento”

Los jugadores podrán completar logros, cada uno con un requisito distinto.

Los logros completados pueden desbloquear nuevos aspectos visuales para las estrellas que se colocan en el canvas de “Mi Firmamento”. De esta forma, los jugadores tendrán más variedad de formas de estrellas con las que decorar su lienzo.

### Reglas de torneo

Como se trata de un juego que también es de carácter competitivo, se aplicarán normas personalizadas exclusivas de torneos. Irán enfocadas a facilitar el trabajo de los organizadores a la hora de implementar reglas para la competición. Algunos ejemplos de opciones personalizadas para torneos son:

Opciones personalizadas para torneo:

Poder generar una semilla exclusiva para ese torneo. Así como también generar un firmamento y scoreboard propio del torneo.

Establecer el número de vidas que tiene un jugador. Por ejemplo: Solo una, dos, tres... hasta cinco, por ejemplo.

O habilitar para que se tenga que completar un porcentaje del mapa en el menor tiempo posible o en un plazo de tiempo. Y registrar el tiempo de los jugadores en el scoreboard.

Hacer que la semilla se habilite en un tiempo concreto.

El scoreboard puede también añadir los tiempos de cada partida jugada y el tiempo total de todas las partidas de un jugador si es que tiene más vidas.

Se deshabilitarán los anuncios durante el torneo, así como la posibilidad de revivir una vez en la partida.

Se proporcionará un código de administrador del torneo al organizador. Y códigos de participación.

Se podrían otorgar cosméticos únicos al ganador o ganadores del torneo.

Los organizadores pueden modificar los porcentajes de los tipos de biomas en la generación de mundo. Además, también tienen la opción de deshabilitar rarezas de objetos u objetos concretos.

### Efectos de sonido y música adicional

Aunque se empiece a desarrollar el apartado musical en la beta, para la versión Golden se implementarán toda la banda sonora y los efectos de sonido. También se tendrá en cuenta la retroalimentación auditiva respecto a la interacción del jugador con el videojuego.

### Bugs resueltos y videojuego

Se llevará a cabo un testeo exhaustivo para localizar y resolver todos los errores de código que puedan surgir. Para prevenir incompatibilidades entre navegadores, se testeará en diversos navegadores.

### Adaptación a distintos dispositivos, navegadores web y controles

El videojuego se adaptará a dispositivos móviles y a los controles de mando. De esta forma, se mejorará la accesibilidad y flexibilidad.

# POST-MORTEM

## Visión individual

### ¿Qué cosas han ido bien?

Carlos: “Hemos logrado que ya estén implementados prácticamente todos los sistemas en los que se basa el juego para funcionar”.

Hemos cumplido la meta de programación para la alfa y todas las funcionalidades clave están implementadas. El esqueleto del juego está hecho, ahora solo hay que llenarlo de contenido. Los cambios de programación que se hagan no serán tan drásticos como lo han sido de cara a la alfa.

Carlos: “Nos hemos logrado poner todos de acuerdo y trabajar con ilusión”.

Las tareas se han dividido bien y no se han solapado entre sí. Además, después de que el quinto miembro del grupo abandonase el equipo, se recobró la ilusión por el proyecto. De pronto, se rompieron todas las cadenas que retenían el diseño y programación del juego y se pudo avanzar a pasos agigantados.

Carlos: “No ha habido ningún problema grande a la hora de testear el juego”.

Toda la estructura del juego a nivel de programación ha estado cohesionada desde el comienzo y se ha aplicado el principio de flexibilidad. Eso ha permitido que no surjan problemas profundos en el código y funcionamiento del juego.

Elena: “Organizar las tareas con Trello ha sido de lo mejor que hemos hecho”.

Ha sido clave el uso la herramienta de Trello para etiquetar las tareas, asignárselas a cada miembro del equipo correspondiente y marcar fechas fijas de entrega. Esto ha facilitado que se cumplan los requisitos para el desarrollo dentro de un plazo establecido y ha mejorado la organización de manera incalculable. La existencia de una fecha límite ha creado una sensación de urgencia que ha incrementado la productividad de los miembros del equipo.

### ¿Qué cosas habría que mejorar?

Carlos: “Hemos tardado mucho en empezar.”

Eso ha provocado que haya menos tiempo y menos gente dedicada a la programación. La próxima vez, las tareas se establecerán con mayor margen y se acortarán las fechas de entrega de cada una de las tareas.

Carlos: “La programación en la alfa ha sido un proceso muy intensivo y bastante agotador.”

Se espera que para el desarrollo de la versión Beta, el trabajo se repartirá mucho más, ya que el trabajo ahora estará mucho más centrado en la creación de assets y contenido para el videojuego.

Elena: “Para la Beta, me gustaría dividir las tareas en sprints”.

Esto haría que no se retrasasen algunas tareas hasta la entrega. Se marcaría un ritmo de trabajo más agilizado, pues los sprints contarían con fecha límite y no se pasaría al siguiente hasta que se completasen todas.

Sandro: “En general, la estimación de tiempo para las tareas no ha sido muy acertado en la fase del prototipo. Muchas terminaron por ocupar un plazo mayor del esperado, incluso llegando a durar el doble”.

“A título personal, considero que he dedicado demasiado tiempo en la fase alpha intentando mejorar los assets al máximo antes de entregarlos al equipo en lugar de utilizar ese tiempo para crear otros diseños más necesarios, como los diseños de los enemigos. También, era la primera vez que hacía animaciones 2d en píxel art, por lo que el tiempo que le dediqué a esa tarea en concreto fue mucho mayor del esperado, y esto inevitablemente hizo que algunas tareas se entregaran con un cierto retraso”.

Otro aspecto que mejorar es el hecho de trabajar en varias tareas al mismo tiempo en lugar de ocuparse de una cada vez. Esta es una práctica que he realizado debido al aspecto creativo de mis tareas (cuando la inspiración para la creación de un asset termina es común que continúe con otra actividad distinta del proyecto). El problema que deriva esta forma de trabajar es que el tiempo de dedicación se distribuye en varias tareas y esto provoca que los diseños se entreguen más tarde y en bloque.

Adrián: “Se deberían hacer más reuniones a lo largo del desarrollo del juego, aunque no todos los participantes pudieran reunirse en alguna de ellas”.

En muchas ocasiones se intentaban realizar reuniones, estableciendo día y hora mediante encuestas, que la mayor parte de las veces no llegaban a nada poque no estábamos todos de acuerdo en el horario. Se podrían haber hecho en muchas ocasiones reuniones con un mínimo de 3 personas, de 4 que somos en el grupo.

Esta situación provoca que no exista muchas veces una evolución en el trabajo y en el reparto de tareas. Se llegaban a dar situaciones donde alguien podía estar sin realizar tareas porque ya las terminó, mientras que otros podrían estar más atacados en alguna tarea y necesitaran algo de ayuda para terminarla a tiempo.

Adrián: “La carga de trabajo es demasiado grande y exigente”

Tras terminar con la entrega del primer prototipo del juego nos dimos cuenta de que la siguiente entrega era demasiado temprana para toda la carga de trabajo que necesitaríamos para poder terminar con el proyecto. Esto es debido a la gran cantidad de contenido que contiene el juego y a la gran ambición de querer hacer muchas características distintas y tan desarrolladas para el tiempo propuesto.

La solución más lógica, según mi criterio, sería reducir el contenido del juego para centrarnos en los aspectos más relevantes, aquellos que en un principio haya que abordar obligatoriamente, y así, luego continuar con el contenido que esté en un segundo plano si diera tiempo. La solución tomada fue una mayor asignación de tareas a cada miembro del grupo en un periodo de tiempo aún menor del que solíamos estimar en la anterior entrega. Esto seguramente haga que muchas tareas no lleguen a completarse ya que el tiempo de desarrollo estimado es ridículamente corto, pudiendo provocar que el desarrollo de la beta no se llegue a concluir o que si lo haga no sea en las mejores condiciones.

La solución aplicada tiene menos sentido sabiendo que la mayoría de las integrantes del grupo tenemos otros trabajos de distintas asignaturas de los que preocuparnos, provocando que los que tengan más tiempo que dedicar a esta asignatura y más nivel / rapidez realizando sus tareas tengan al final mayor carga de trabajo que los demás.

### Retroalimentación externa

Carlos: “Temo que las personas ajenas al game design intenten imponer sus ideas”.

Es una preocupación que compartimos los miembros del equipo de diseño de juego antes incluso se iniciase el proyecto, sin siquiera conocer aún a los que iban a ser el resto de integrantes. Y así sucedió, nos surgió ese problema. Así que decidimos pedir ayuda externa.

Profesor: “Mi recomendación es que hagáis lo que se suele hacer en un entorno profesional: proponed los encargados de diseño las ideas y sometedlas a votación del resto del grupo.”

Ese mismo día, se pusieron a votación todas las mecánicas ideadas. Dos días después, nadie había votado. Por lo tanto, se decidió mantenerlas para no prorrogar aún más el desarrollo.

La idea inicial del videojuego era muy general, y el sistema de scoreboard era idéntico al de otros juegos.

Madre de Integrante: “Me parece un juego muy aburrido. Yo no le veo nada de competitivo porque yo no sé qué es lo que están haciendo los demás.”

Ese fue el recibimiento que recibió la idea que se tenía al comienzo. Faltaba motivación, promover el espíritu competitivo. Y eso era algo que no se iba lograr solo con números, con un scoreboard tradicional.

Así que se diseñó la idea de mostrar el scoreboard mediante un firmamento, donde cada estrella representa de forma visual con su tamaño la puntuación aproximada de cada jugador. Este nuevo diseño fue todo un acierto, y satisfizo la necesidad de fomentar el aspecto competitivo del juego.

## Visión grupal

### ¿Qué cosas han ido mal?

Todos los integrantes del equipo de desarrollo coincidimos en que el desarrollo del videojuego ha sido mayormente de carácter caótico. Y el principal motivo es que fue enormemente ralentizado por un miembro que ya no forma parte del equipo. Debido a las críticas tempranas del diseño, a la poca flexibilidad sobre la idea del juego y su énfasis en que no se realicen prototipos, se prorrogó la programación hasta pasadas dos semanas. Además, no comunicó al grupo que mientras tanto, se encontraba haciendo un prototipo muy distinto a las ideas que tardaron horas intensas de reuniones en establecerse.

Al día siguiente, e incluso a la semana siguiente, todo lo que se había establecido es como si no existiera para el compañero. Además, tachó de incompetentes a algunos miembros del equipo, tergiversando lo que se hablaba y tratando irrespetuosamente a los demás integrantes.

Es entonces cuando decidimos calmar la situación y pedir ayuda al profesor. Desde antes del proyecto, temíamos que el apartado de diseño del juego se viese solapado por miembros que no tuviesen el rol de game design. Y así fue, cada idea tenía que pasar por múltiples filtros y se tardó en establecer el funcionamiento general del juego. Para solventarlo, los profesores nos aconsejaron aplicar un sistema profesional de encuestas, donde las ideas se aprobarían o denegarían entre todos los miembros del grupo. Así se hizo, pero nadie externo al equipo de game design votó.

Una vez el compañero decidió marcharse del equipo, retomamos con fuerza el proyecto, ahora sin los impedimentos constantes del excompañero. Los miembros del equipo reavivamos nuestra ilusión por realizar el proyecto y le dedicamos mucho tiempo, todo para paliar el lapso de dos semanas de reuniones infructíferas y bloqueo de la programación. Se avanzó a pasos agigantados con el diseño de juego y el prototipado, así como en muchos otros aspectos relacionados con el desarrollo, como por ejemplo el mantener las redes sociales al día.

Sin embargo, con el tiempo surgió un grave problema, era el que iba a ser el programador principal el que había abandonado del equipo. Y, debido a ello, el eslabón de programación del videojuego se encuentra actualmente muy débil, compuesto por solo un compañero con amplios conocimientos en Unity. El resto del equipo trata de liberarle carga de trabajo, pero se requiere de un mayor nivel de programación para alcanzar las exigencias del videojuego.

Al ir añadiendo complejidad mediante mayor número de elementos y características distintas al juego se consigue que la carga de trabajo sea demasiado grande teniendo en cuenta los tiempos establecidos de entregas y el tiempo disponible para afrontar las demás asignaturas.

### Lecciones aprendidas

En primer lugar, antes de conformar un grupo de desarrollo, sería necesario que investigásemos sobre cada uno de los integrantes. Es importante averiguar sus fortalezas y debilidades respecto al desarrollo del producto. Y así se hizo, todos los sectores: diseño, arte y programación; quedaron cubiertos y en manos competentes. Sin embargo, aunque se lograse esa estabilidad, pasamos completamente por alto la capacidad de trabajo en equipo de los integrantes. Solo conocíamos resultados de prácticas anteriores, sin indagar realmente en el comportamiento y actitud de cada uno de ellos. Deberíamos haber consultado a compañeros sobre su experiencia de trabajo con cada uno de los miembros antes de tomar decisiones definitivas.

En segundo lugar, la mayoría de los integrantes deberíamos asentar mejor las bases de programación. Antes de enfrentar un proyecto de tal magnitud, deberíamos de haber partido de un nivel de programación a la altura de la exigencia del código del videojuego. Una posible solución es que más allá de dedicarnos a las tareas de desarrollo relacionadas con nuestra rama, podríamos emplear el tiempo restante en formarnos aprendiendo correctamente el lenguaje de C# y el cómo usar Unity en profundidad.

También podríamos haber tenido en cuenta las cualidades y limitaciones de cada integrante del equipo a la hora de realizar el contenido del videojuego para haber podido aprovechar mejor su potencial. Ya que no todos los integrantes parten con la mejor base en programación, se podría haber enfocado el videojuego mediante mecánicas más accesibles y resaltando aún más otros aspectos, sin dejar que la ambición en cuanto a contenido llegue a abrumar el desarrollo.

# MODELO DE NEGOCIO

## Público objetivo

Tanto jugadores casuales como jugadores competitivos. El juego se puede jugar sin forzar a los jugadores a participar de forma competitiva, pero el alto skill cap y el sistema de puntuaciones promueven la competitividad entre los jugadores más dedicados, permitiendo que todo tipo de jugadores encuentren algo que disfrutar en este juego.

Está enfocado a personas que no tienen grandes gastos de dinero y que disfrutan ayudando a los demás.

El modelo de negocio que sigue el juego de Candelight es el de cebo y anzuelo. El juego se ofrece sin ningún coste y los beneficios se obtendrán a partir de las micro transacciones, las donaciones y los anuncios (incluyendo promociones).

## Mapa de empatía

### ¿Qué piensa y siente?

El público objetivo piensa que ocurren muchas injusticias a diario y siente la necesidad de ayudar. Es por ello, que suelen participar en donaciones. Pero no se limitan solo a aportar financiación a ONGs, sino que a veces se atreven a donar abnegadamente cosas valiosas para ellos, como por ejemplo juguetes y ropa que ya no usen.

Lo importante para ellos es aportar su granito de arena. Y si lo pueden hacer reciclando, participando en voluntariados, recogiendo los desperdicios en picnics o ayudando en la casa; lo hacen sin darle demasiadas vueltas.

Quiere ayudar en su comunidad y sentir que sus esfuerzos tienen consecuencias positivas en lo que le rodea. Les gusta ver que, de una forma u otra, sus acciones derivan en resultados positivos.

Por ejemplo, si pudiesen ver una imagen o vídeo de las personas haciendo uso de su ayuda, les insuflaría ánimos y llenaría por dentro. Sin embargo, aún sin contar con pruebas de que lo que hacen por lo demás es de utilidad, sin recibir nada a cambio, siguen esforzándose.

A nuestro público le jugar a videojuegos en sus ratos libres. Eso incluye el probar nuevos títulos creados por desarrolladores indie, videojuegos antiguos o videojuegos que, en general, estén infravalorados y sean poco conocidos. Lo que más les gusta es encontrar pequeñas “gemas ocultas” que poder recomendar a sus amigos.

### ¿Qué oye?

Habla con gente cercana sobre su progreso en juegos que comparten, además de competir por ver quién ha logrado avanzar más. Le recomiendan de vez en cuando muchos juegos, de los cuáles ha habido unos cuántos que ya ha jugado. Pero siempre escucha rumores de algún juego independiente que aún no ha probado.

Presta atención a lo que juegan sus amigos. Si les gusta jugar a un juego online, estará al tanto de las novedades y actualizaciones. Además, sus amigos no solo le proponen jugar a juegos de ordenador, sino también a videojuegos para móvil y consola. Muchos de estos juegos son de carácter crossplay, para que puedan jugar juntos sin importar desde qué dispositivo lo hagan.

### ¿Qué ve?

Día a día entra a las redes sociales para ponerse en contacto con las novedades. Se informa mucho sobre lo que ocurre en el mundo e incluso ve a su familia hablar de temas importantes que ocurren en la actualidad. Normalmente cuando presencia esto es en la hora de cenar, cuando toda su familia está reunida.

Al principio podía no comprender los temas que se trataban, no le interesaba situación de la sociedad. Pero ahora presta atención y escucha a su familia para poder intervenir y así aportar su granito de arena de información de la que se ha enterado en las redes.

También consume mucho contenido en internet, sobre todo en Youtube. Le gusta ver vídeos de reseñas de videojuegos indies y pequeños documentales sociopolíticos, a veces de tono humorístico.

### ¿Qué dice y hace?

Disfruta tanto de experiencias de un jugador como multijugador. Y, siempre que puede, juega con sus amigos a juegos cooperativos y competitivos. Aunque, en el fondo, también disfruta teniendo momentos privados en donde lee, escribe y prueba uno de los muchos títulos de videojuegos que tiene “pendientes”. Pues, aunque jugar con sus amigos le parezca muy divertido, entretenido y le ayude a desconectar; es este su pequeño rato de tranquilidad.

Aun así, no suele dedicar mucho a cuidarse de sí mismo, pues prioriza el ayudar a otros con sus tareas o participar en eventos. De vez en cuando asiste a huelgas, pero solo las que de verdad apoya; nunca sigue ciegamente lo que le digan, pero está abierto a sugerencias y otras perspectivas para modificar y ser flexible con la suya propia.

Participa también en voluntariados y, aunque a veces tienda a ser muy casero, sí que da la oportunidad a probar experiencias nuevas y a viajar. A su vez, también le gusta explorar y pasear por el campo, por lugares tranquilos donde pueda meditar en calma.

No dispone de todo el ocio que desearía ya que estudia o trabaja. Y tiene un horario muy ocupado, lleno de planes y tareas. E incluso, si le surge alguna ocasión en donde tenga que ayudar a algún amigo o familiar, no le importa añadir esa tarea a su calendario. Es por ello por lo que solo puede jugar en sus ratos libres.

## Caja de herramientas

Establecer una Caja de Herramientas ayudará a saber estimar si el proyecto es viable y factible de realizar, así como de vender y distribuir. Además, de esta forma se justifica su desarrollo y se establecen las relaciones que se planean mantener con influencia externa. Y ayuda a discernir las fortalezas y debilidades para predecir si el proyecto puede salir adelante.

En la Caja de herramientas se detalla la relación económica con los clientes, que en nuestro caso serán los jugadores; los proveedores, que para el juego serán los servidores; la comunidad y las ONGs.

En resumen, estos son los bloques que consideramos en nuestra caja de herramientas:

El estudio, Velorum; los clientes, jugadores; proveedores, servidores; comunidad, con influencers, un foro y las redes sociales y ONGs (con posibles candidatas: Light Humanity, Energía sin Fronteras, Electricistas sin Fronteras, Energía Responsable).

He aquí un esquema que detalla cuáles van a ser las interrelaciones entre cada uno de los elementos que se han incluido en la Caja de Herramientas:

## Tal y como se aprecia en el esquema, el estudio, Velorum, se comunicará con cuatro entidades distintas: Clientes, Comunidad, Servidores y ONG.

En el caso de la relación con los clientes, ellos aportarán financiación al proyecto mediante micro transacciones a cambio de que les ofrezcamos cosméticos y elementos de personalización; aportarán un poco de dinero al visualizar los anuncios y mediante donaciones, pues un porcentaje será recaudado para el estudio; en orden de cantidad de efectivos.

Los servidores nos aportarán su servicio y soporte online para el scoreboard a cambio de una cantidad de dinero.

En cuanto a las ONGs, no aportarán ningún beneficio físico como tal al juego, pero su nombre puede ayudar a ensalzar el prestigio del videojuego y a que sea más conocido. Además, su colaboración se compagina a la perfección con el mensaje de abnegación del videojuego.

Por último, decir que se planea realizar unas inversiones para pagar a influencers a cambio de que promocionen Candelight. Y, quizás, en un futuro, se ve recompensado con un pequeño beneficio económico de colaboraciones con el juego.

## Lienzo o Canvas

### Socios Clave

\- Unity: Es la herramienta que más está usando el equipo para desarrollar el videojuego. Se trata del motor de videojuegos en el que gira todo el proyecto.

\- Itch.io: Es la plataforma en donde se publican oficialmente las versiones del videojuego. De esta forma, pueden llegar al público de forma gratuita.

\- Influencers: Se planea contactar con algún Youtuber de pequeña a media relevancia para promocionar el juego.

### Actividades Clave

\- Desarrollo del videojuego: Es la actividad clave más importante. Es necesario cubrir todos los aspectos del juego, desde su diseño, narrativa, programación y apartado artístico. Requerirá el mayor esfuerzo de todas las tareas clave y es vital para que el producto salga adelante.

\- De la página web: se accederá a ella a través del porfolio y recopilará información sobre cada miembro del equipo y un enlace a las redes sociales del estudio.

\- Implementar y gestionar un scoreboard online: Las puntuaciones de los jugadores quedarán publicadas y podrán ser vistas por otros jugadores. De esta forma, se pretende fomentar el espíritu competitivo del juego.

\- Relación a través de un foro: Se tiene pensado abrir un foro en el que las personas puedan escribir sus opiniones e impresiones sobre el videojuego. Allí también podrán compartir experiencias sin tener que limitarse a las formalidades que pueda conllevar el publicar una reseña en Itch.io.

\- De las redes sociales: Se llevará al día la gestión de las redes sociales, donde se publicarán actualizaciones del avance del desarrollo del videojuego. De esta forma, se pretende crear un público base que vaya a jugar el juego en su versión definitiva.

### Recursos Clave

\- Unity: Es el motor de videojuego que se ha decidido usar para el desarrollo del videojuego, por ser el más asequible e intuitivo de dominio público. Todos los elementos del videojuego convergerán en el motor y los scripts de #C quedarán vinculados a los componentes pertinentes de los GameObjects.

\- Licencias de software necesarias para el desarrollo: programas de edición de fotografía (por ejemplo: Photoshop, Procreate, Aseprite), programas para modelado 3D y animación (como Blender o 3DS Max), programas para el prototipado de interfaces (como Figma o Sketch).

### Propuesta de Valor

\- Juego para navegadores web y free-to-play (F2P).

\- Juego de magia con mecánicas que permiten al jugador realizar combinaciones de elementos, construyendo conjuros personalizados en tiempo real.

\- Combinación de técnicas 2D con detalles 3D.

\- Personalizaciones visuales del personaje.

\- Dispone de un scoreboard interactivo a modo de mapa.

\- Ambientación, historia y world-building originales.

### Relación con el Cliente

\- Soporte y atención con el cliente a través del foro: Además de permitir a los jugadores hacer comentarios sobre el juego en el foro, también servirá como herramienta para que reporten errores que tenga el videojuego.

\- Relación a través de las siguientes redes sociales oficiales de la compañía: Reddit, Twitter (X), YouTube, Instagram, Facebook, TikTok e itch.io. Se trabajará en las redes sociales de difusión de manera simultánea y llevando a cabo actualizaciones periódicas.

### Canales de Distribución

\- Itch.io: Es la plataforma principal donde se publicará el videojuego, pues es una página web ideal para que los desarrolladores de videojuegos independientes hagan públicos sus proyectos.

\- Página web oficial: En la página web vendrá un enlace al porfolio para que se pueda consultar la información sobre cada uno de los miembros del equipo. además, en la web figurará información sobre el videojuego Candelight que puede resultar útil para exponer el proyecto al público. Se incluirá a su vez un enlace al videojuego en Itch.io.

\- Redes sociales: Servirá para que el enlace al videojuego llegue a un mayor alcance de público y más personas se vean interesadas por probar el videojuego.

### Segmentos de Clientes

Se busca a jugadores de perfil competitivo; es decir, el juego cuenta con un alto skill cap para jugadores que busquen una experiencia desafiante. Además, la existencia del scoreboard permite que los jugadores compartan puntuaciones entre ellos, creándose una rivalidad.

La intención es que el juego se sencillo de aprender, pero que permita a los jugadores más experimentados no dejar de mejorar. Está pensado para que desarrollen sus propias estrategias y se reten a sí mismos a la hora de ver hasta qué punto del juego pueden avanzar sin morir.

Cabe mencionar que también se busca a un público casual, que busque partidas rápidas para desestresarse y desconectarse de la realidad, aunque sea por un rato. Es por ello que siempre se complete un nivel se guardará el progreso, para que puedan avanzar y seguir jugando cuando quieran sin darle importancia al poco tiempo que le dediquen.

Es más, el videojuego contará con tutorial para hacer ameno el primer contacto con el videojuego y el HUD será intuitivo para que se sepan manejar, aunque ya lleven un tiempo sin jugar al juego.

Por último, hay que mencionar que el juego está pensado para un rango de edad juvenil. Sin embargo, eso no previene que otro tipo de jugadores se interesen por él. Su amplio apartado narrativo, estético y de mecánicas puede atraer realmente a todo tipo de jugadores que busques una experiencia concreta y plena.

### Estructura de Costes

**\- Costes de mantenimiento de servidores (hosting):**

Es preciso mantener los servidores para que el scoreboard quede registrado y pueda actualizarse siempre que se tenga conexión a internet.

**\- Coste del dominio web:**

Si se decidiese usar una página que no fuese Github Pages, se requería de un dominio de pago para poder mantener pública la página web.

**\- Costes de marketing:**

Incluye el pago a los influencers y la publicidad en las redes sociales.

### Fuentes de Ingresos

\- Micro transacciones:

Los jugadores podrán comprar desde el juego skins personalizadas y mecánicas que solo afecten a su experiencia individual, no a nivel competitivo.

\- Publicidad:

Se incluirán anuncios (imágenes, vídeos y banners) tanto en la página web como en el propio juego y en vídeos promocionales.

\- Donaciones:

Se recaudará un porcentaje del 5% de las donaciones a una ONG con la que se decida colaborar.

# MONETIZACIÓN

Se trata de un juego free-to-play con micro transacciones con skins; donaciones (a través de Paypal, tarjeta bancaria y Bizum); y publicidad a través de Google Adsense (vídeos publicitarios al morir, banner en el juego, anuncios en la página web).

Las micro transacciones pueden ser utilizadas para ofrecer cambios visuales como skins, ofreciendo equipamiento con distintas ventajas y desventajas que cambien ligeramente cómo juega el jugador (evitar que los objetos de pago resulten una ventaja esencial para pasar el juego, pues no es un juego pay-to-win).

Ejemplos: Vestimentas con ventajas y desventajas que potencian ciertos tipos de ataques, pero deshabilitan otros, permitir continuar la partida viendo un anuncio al ser derrotado, pero solo contemplando avances personales (no se registra la puntuación en el ranking), etc…

La opción de revivir a cambio de ver un anuncio solo aparecerá una vez por partida.

Ayudas/Donaciones, a ONGs relacionadas con la temática del juego, como Light Humanity o Energía Sin Fronteras.

# REDES SOCIALES

## Cuenta de Velorum

Las redes sociales que se han utilizado son:

**Tik Tok:** [https://www.tiktok.com/@velorumgames?\_t=8qN7TZzLAMS&\_r=1](https://www.tiktok.com/@velorumgames?_t=8qN7TZzLAMS&_r=1)

**Youtube:** [https://www.youtube.com/channel/UCn3J\_JFLRmhUvrpEv95pU3A](https://www.youtube.com/channel/UCn3J_JFLRmhUvrpEv95pU3A)

**Twitter:** [https://x.com/VelorumGames](https://x.com/VelorumGames)

**Instagram:** [https://www.instagram.com/velorum\_games/](https://www.instagram.com/velorum_games/)

La imagen del icono empleado para la foto de perfil:

## Posts

**Vídeo presentación del estudio:**

[https://urjc.sharepoint.com/sites/gr\_2024.2175\_2175038\_AT-VELLORUM/\_layouts/15/stream.aspx?id=%2Fsites%2Fgr%5F2024%2E2175%5F2175038%5FAT%2DVELLORUM%2FDocumentos%20compartidos%2FVELORUM%2Fvelorum%20presentacion%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2E8595f2fd%2Df58f%2D4332%2D9236%2D2cef368818d9](https://urjc.sharepoint.com/sites/gr_2024.2175_2175038_AT-VELLORUM/_layouts/15/stream.aspx?id=%2Fsites%2Fgr%5F2024%2E2175%5F2175038%5FAT%2DVELLORUM%2FDocumentos%20compartidos%2FVELORUM%2Fvelorum%20presentacion%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2E8595f2fd%2Df58f%2D4332%2D9236%2D2cef368818d9)

**Presentación del equipo y roles:**

**Concept Art del videojuego:**

\- Bocetos iniciales de la Sombra y estudio de color:

**\- Concept Art Murciélago Gigante:**

**\- Bocetos iniciales y diseño final del Hombre de Cobre:**

**\- Vídeo anuncio del videojuego:**

[**https://urjc.sharepoint.com/sites/gr\_2024.2175\_2175038\_AT-VELLORUM/\_layouts/15/stream.aspx?id=%2Fsites%2Fgr%5F2024%2E2175%5F2175038%5FAT%2DVELLORUM%2FDocumentos%20compartidos%2FVELORUM%2FRedes%20sociales%2FCandelight%2Fanuncio%20candelight%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2Ec0e7d719%2Dfd7d%2D4e7d%2D9556%2D65c922c1b3b0**](https://urjc.sharepoint.com/sites/gr_2024.2175_2175038_AT-VELLORUM/_layouts/15/stream.aspx?id=%2Fsites%2Fgr%5F2024%2E2175%5F2175038%5FAT%2DVELLORUM%2FDocumentos%20compartidos%2FVELORUM%2FRedes%20sociales%2FCandelight%2Fanuncio%20candelight%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2Ec0e7d719%2Dfd7d%2D4e7d%2D9556%2D65c922c1b3b0)

**\- Teaser novedades:**
