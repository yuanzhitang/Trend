<template>
    <!--top-Header-menu-->
    <nav>
        <Header></Header>
        <div id="user-nav" class="navbar navbar-inverse">
            <ul class="nav">
                <li class="">
                    <a title="" href="#">
                        <i class="icon icon-cog"></i> 
                        <span class="text"> {{ dateNow }}</span>
                    </a>
                </li>

                <li class="dropdown" id="menu-messages">
                    <a href="#" data-toggle="dropdown" data-target="#menu-messages" class="dropdown-toggle">
                        <i class="icon icon-envelope"></i> 
                        <span class="text">View Projects</span> 
                        <span class="label label-important">5</span> 
                        <b class="caret"></b>
                    </a>
                    <ul class="dropdown-menu">
                        <li>
                            <a class="sAdd" title="" href="#">
                                <i class="icon-plus"></i> Data Exchange 5.0
                            </a>
                        </li>
                    </ul>
                </li>
                
                <li class="">
                    <a title="" href="~/Settings/Index">
                        <i class="icon icon-user"></i> 
                        <span class="text">Availability</span>
                    </a>
                </li>
            </ul>
        </div>
        <!--close-top-Header-menu-->
    </nav>
</template>

<script>
import Header from "../components/Header.vue";

export default {
  components: { Header },
  data() {
    return {
      dateNow: null
    };
  },
  created() {
    var date = new Date();
    this.dateNow = date.toLocaleTimeString();

    let excanvasjs = document.createElement('script')
    excanvasjs.setAttribute('src', '/assets/js/excanvas.min.js')
    document.body.appendChild(excanvasjs)
    let jqueryjs = document.createElement('script')
    jqueryjs.setAttribute('src', '/assets/js/jquery.min.js')
    document.body.appendChild(jqueryjs)
    let jqueryui = document.createElement('script')
    jqueryui.setAttribute('src', '/assets/js/jquery.ui.custom.js')
    document.body.appendChild(jqueryui)
    let bootstrapjs = document.createElement('script')
    bootstrapjs.setAttribute('src', '/assets/js/bootstrap.min.js')
    document.body.appendChild(bootstrapjs)
    let jqueryflot = document.createElement('script')
    jqueryflot.setAttribute('src', '/assets/js/jquery.flot.min.js')
    document.body.appendChild(jqueryflot)
    let jqueryflotresize = document.createElement('script')
    jqueryflotresize.setAttribute('src', '/assets/js/jquery.flot.resize.min.js')
    document.body.appendChild(jqueryflotresize)
    let jquerypeity = document.createElement('script')
    jquerypeity.setAttribute('src', '/assets/js/jquery.peity.min.js')
    document.body.appendChild(jquerypeity)
    let jqueryuniform = document.createElement('script')
    jqueryuniform.setAttribute('src', '/assets/js/jquery.uniform.js')
    document.body.appendChild(jqueryuniform)
    let select2 = document.createElement('script')
    select2.setAttribute('src', '/assets/js/select2.min.js')
    document.body.appendChild(select2)
    let jquerydataTables = document.createElement('script')
    jquerydataTables.setAttribute('src', '/assets/js/jquery.dataTables.min.js')
    document.body.appendChild(jquerydataTables)
    let matrix = document.createElement('script')
    matrix.setAttribute('src', '/assets/js/matrix.js')
    document.body.appendChild(matrix)
    let matrixdashboard = document.createElement('script')
    matrixdashboard.setAttribute('src', '/assets/js/matrix.dashboard.js')
    document.body.appendChild(matrixdashboard)
    let jquerygritter = document.createElement('script')
    jquerygritter.setAttribute('src', '/assets/js/jquery.gritter.min.js')
    document.body.appendChild(jquerygritter)
    let matrixinterface = document.createElement('script')
    matrixinterface.setAttribute('src', '/assets/js/matrix.interface.js')
    document.body.appendChild(matrixinterface)
    let matrixchat = document.createElement('script')
    matrixchat.setAttribute('src', '/assets/js/matrix.chat.js')
    document.body.appendChild(matrixchat)
    let jqueryvalidate = document.createElement('script')
    jqueryvalidate.setAttribute('src', '/assets/js/jquery.validate.js')
    document.body.appendChild(jqueryvalidate)
    let matrixform_validation = document.createElement('script')
    matrixform_validation.setAttribute('src', '/assets/js/matrix.form_validation.js')
    document.body.appendChild(matrixform_validation)
    let jquerywizard = document.createElement('script')
    jquerywizard.setAttribute('src', '/assets/js/jquery.wizard.js')
    document.body.appendChild(jquerywizard)
    let matrixpopover = document.createElement('script')
    matrixpopover.setAttribute('src', '/assets/js/matrix.popover.js')
    document.body.appendChild(matrixpopover)
    let matrixtables = document.createElement('script')
    matrixtables.setAttribute('src', '/assets/js/matrix.tables.js')
    document.body.appendChild(matrixtables)

    let jquery1102 = document.createElement('script')
    jquery1102.setAttribute('src', '/assets/js/jquery-1.10.2.js')
    document.body.appendChild(jquery1102)
    let bootstrap = document.createElement('script')
    bootstrap.setAttribute('src', '/assets/js/bootstrap.js')
    document.body.appendChild(bootstrap)
    let respond = document.createElement('script')
    respond.setAttribute('src', '/assets/js/respond.js')
    document.body.appendChild(respond)
  },
  computed: {
    timeNow: function () {
      return new Date().toLocaleTimeString();
    }
  }
}

</script>

<style>
@import '../assets/css/bootstrap.min.css';
@import '../assets/css/bootstrap-responsive.min.css';
@import '../assets/css/matrix-style.css';
@import '../assets/css/matrix-media.css';
/* @import '../assets/fonts/font-awesome/css/font-awesome.css'; */

/* @import '../assets/css/fullcalendar.css'; */
@import '../assets/css/jquery.gritter.css'; 

</style>