<template>
  <div id="app">
    <Nav></Nav>
    <!-- <img src="./assets/logo.png"> -->
    <router-view/>
  </div>
</template>

<script>
import Nav from "./components/Nav.vue";

export default {
  name: 'App',
  components: { Nav }
}
</script>

<style>
@import 'assets/css/bootstrap.min.css';
@import 'assets/css/bootstrap-responsive.min.css';
@import 'assets/css/matrix-style.css';
@import 'assets/css/matrix-media.css';
/* @import 'assets/fonts/font-awesome/css/font-awesome.css'; */
/* @import 'assets/css/fullcalendar.css'; */
@import 'assets/css/jquery.gritter.css'; 

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
