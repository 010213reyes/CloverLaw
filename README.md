# 2D Action Platformer - Proyecto Base Unity

Este repositorio contiene el código fuente para un juego de plataformas de acción en 2D (estilo Metroidvania/Hack & Slash) desarrollado en Unity. El proyecto destaca por su arquitectura modular, el uso de interfaces genéricas para sistemas de combate y controladores independientes para efectos visuales, audio y físicas.

## 🏗️ Arquitectura del Proyecto

El código está organizado en distintos módulos y sistemas para facilitar su mantenimiento y escalabilidad:

### 🦸‍♂️ Personaje Principal (Hero)
* **`HeroController.cs`**: Controlador central del jugador. Gestiona la lógica de movimiento, saltos impulsados por físicas, doble salto, ataques, orientación automática (flip) y recepción de daño con sistema de retroceso (knockback) y tiempo de invulnerabilidad.

### ⚔️ Sistema de Combate (Weapons)
* **`ITargetCombat.cs`**: Interfaz unificada (`TakeDamage`) que debe implementar cualquier entidad que pueda recibir daño en el juego (enemigos, héroe, objetos destructibles).
* **`SwordController.cs`**: Componente que controla los frames activos del ataque del jugador, habilitando temporalmente un hitbox (`Collider2D` como trigger) para aplicar daño a los enemigos.
* **`DamageController.cs`**: Gestor de áreas de daño genéricas gestionadas por un Trigger para dañar al jugador u otros entes.
* **`DamageDemo.cs`**: Entidad demostrativa (enemigo base) que aplica la interfaz `ITargetCombat` para restarse vida al interactuar con las armas.

### ⚛️ Físicas y Colisiones (Physics)
* **`LayerChecker.cs`**: Sistema avanzado de detección de superficies (Grounding) utilizando `Physics2D.Raycast`. El héroe usa dos de estos (footA y footB) para una detección precisa en los bordes de las plataformas.
* **`TagId.cs`**: Utilidad para el manejo fuertemente tipado de las etiquetas (Tags) de Unity, evitando errores tipográficos en las colisiones de `OnTriggerEnter2D`.

### ✨ Efectos Visuales (Visual Effects)
* **`DamageFeedbackEffects.cs`**: Controlador que interactúa directamente con los `Material` y Shaders para generar un efecto de parpadeo (Flash Effect) blanco brillante en los sprites cuando reciben un impacto o son invulnerables temporalmente.

### 🔊 Sistema de Audio (Audio)
* **`AudioManager.cs`**: Gestor de audio global que utiliza el patrón *Singleton* y `DontDestroyOnLoad`. Permite reproducir múltiples efectos de sonido (SFX) superpuestos y controlar dinámicamente el volumen de la música (Music) desde cualquier punto del juego sin interrupciones entre escenas.

### 🎥 Cámara y Entorno (Camera & Levels)
* **`CameraController.cs`**: Sistema para que la cámara siga al jugador con un movimiento suave.
* **`ParallaxController.cs`**: Gestiona el desplazamiento diferencial de las capas del fondo para generar una ilusión óptica de profundidad 3D en un entorno 2D.
* **`LevelManager.cs`**: Gestor general encargado de las transiciones, el reinicio de las fases y el flujo del juego.

### 🎬 Animación (Animator)
* **`AnimatorController.cs`**: Script mediador que maneja las transiciones del componente de animación nativo de Unity a través de identificadores de estado limpios (IDs), desligando la lógica directa de los strings de animación.

## 🌍 Entorno y Arquitectura de Scripts (Unity)

Este proyecto está construido en Unity (editor 2022.3.x) con scripts en C# y físicas 2D. La arquitectura se apoya en componentes `MonoBehaviour` que coordinan entrada, animación, físicas y coleccionables a través de eventos de Unity y referencias serializadas en el Inspector.

### Componentes Principales
- **`HeroController`**: Gestiona la entrada del jugador, movimiento, salto, ataques, uso de power-ups y estados (por ejemplo, recuperación de daño). Orquesta animaciones mediante `AnimatorController` y aplica fuerzas con `Rigidbody2D`.
- **`TorchController`**: Controla el comportamiento de la antorcha (enemigo/objeto interactivo), incluyendo colisiones y lanzamiento de objetos/efectos (según configuración del prefab).
- **`PowerUpPickup`**: Administra la recogida de pociones. Detecta interacción con el jugador y aplica el `PowerUpId` correspondiente, destruyendo el objeto tras su uso.
- **`CoinController` y `HeartController`**: Controlan la recogida de monedas y corazones. Al detectar al jugador, actualizan el estado del héroe (monedas o vida) y reproducen efectos de sonido.
- **`WayPoints` y `FollowWayPoints`**: `WayPoints` define un conjunto de puntos (hijos del objeto) y provee `GetNextPoint()`; `FollowWayPoints` consume esos puntos para mover entidades siguiendo rutas predefinidas.

### Flujo de Interacción entre Sistemas
1. **Entrada del jugador** → `HeroController` lee inputs y decide acciones (mover, saltar, atacar, usar power-up).
2. **Físicas 2D** → `Rigidbody2D` aplica fuerzas y colisiones. Los coleccionables usan colisionadores para interactuar con el jugador.
3. **Animación** → `HeroController` dispara estados de `AnimatorController` para sincronizar lógica y visuales.
4. **Coleccionables** → `CoinController`, `HeartController` y `PowerUpPickup` actualizan el estado del héroe y reproducen SFX.
5. **Rutas** → `WayPoints` actúa como repositorio de puntos y `FollowWayPoints` usa esos puntos para guiar movimiento de entidades.

### Recomendaciones de Configuración
- Usar `Rigidbody2D` en `Dynamic` para objetos que deban caer y colisionar con el suelo.
- Configurar colisionadores (Trigger vs. físicos) según el tipo de interacción deseada.
- Mantener referencias serializadas en el Inspector para enlazar `HeroController` con armas, lanzadores y efectos.

## 🛠️ Especificaciones Técnicas
* **Motor Recomendado:** Unity 2022+
* **Lenguaje:** C# 9.0
* **Target Framework:** .NET Framework 4.7.1
* **Dimensiones:** Físicas e interacciones basadas estrictamente en entornos 2D (`Rigidbody2D`, `Collider2D`).

## 🚀 Cómo Empezar

Para comenzar a trabajar con este proyecto, sigue estos pasos:

1. **Clona el repositorio:**
   git clone https://github.com/tu_usuario/tu_repositorio.git

2. **Abre el proyecto en Unity:**
Asegúrate de tener la versión recomendada de Unity instalada. Abre el proyecto desde el directorio clonado.

3. **Configura el entorno:**
Asegúrate de que todas las dependencias y configuraciones necesarias estén en su lugar. Consulta la documentación de Unity para más detalles sobre la configuración del entorno.

4. **Ejecuta el juego:**
Presiona el botón de "Play" en Unity para comenzar a jugar y probar las funcionalidades del proyecto.

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.

