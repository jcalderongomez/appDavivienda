import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EditarComponent } from '../editar/editar.component';
import { Cliente } from '../interfaces/IDataCliente';
import { ClienteService } from '../cliente.service';  

@Component({
  selector: 'app-listar',
  templateUrl: './listar.component.html',
  styleUrls: ['./listar.component.css']
})
export class ListarComponent implements OnInit {
  public load: boolean;
 
  displayedColumns: string[] = ['id','cedula','nombre','apellido','direccion','telefono', 'accion']
  constructor(private clienteService: ClienteService,
    private dialog: MatDialog) { 
      this.load=false;
    }

  ngOnInit(): void {
    setTimeout(() => {
      this.load = true;
    }, 5000);

    this.clienteService.listarClientes();
  }

  get resultados(){
    return this.clienteService.resultados;
  }

  actualizarCliente(cliente: Cliente){
    this.dialog.open(EditarComponent,{
      data:{
        cedula    : cliente.cedula,
        nombre    : cliente.nombre,
        apellido  : cliente.apellido,
        direccion : cliente.direccion,
        telefono  : cliente.telefono,
        id        : cliente.id
      }
    });
  }
}