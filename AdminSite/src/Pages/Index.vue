<template>
  <div>
    <Nav></Nav>
    <Footer></Footer>
  </div>
</template>

<script>

import Header from "../components/Header.vue";
import Footer from "../components/Footer.vue";
import Nav from "../components/Nav.vue";

export default {
  components: { Header, Footer, Nav },
  data() {
    return {
      mydata: {}
    };
  },
  created() {
    let excanvasjs = document.createElement('script')
    excanvasjs.setAttribute('src', '/assets/js/excanvas.min.js')
    document.head.appendChild(excanvasjs)


  },
}
</script>

<style>
@import '../assets/css/bootstrap.min.css';
@import '../assets/css/bootstrap-responsive.min.css';
@import '../assets/css/fullcalendar.css';
@import '../assets/css/matrix-style.css';
@import '../assets/css/matrix-media.css';
@import '../assets/fonts/font-awesome/css/font-awesome.css';
@import '../assets/css/jquery.gritter.css';
</style>