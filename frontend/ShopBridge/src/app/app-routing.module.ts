import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { ItemComponent } from './item/item.component';

const routes: Routes = [  
  { path: '', redirectTo: 'item', pathMatch: 'full'},  
  { path: 'item', component: ItemComponent },  
  { path: 'about', component: AboutComponent },  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
