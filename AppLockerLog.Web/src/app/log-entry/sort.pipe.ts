import { Pipe, PipeTransform } from "@angular/core";
import { LogEntry } from '../shared/log-entry.model';


@Pipe({
  name: 'sort',
  pure: false
})
export class SortPipe implements PipeTransform {
    transform(arr: LogEntry[], path: string[], order: number): LogEntry[] {
        if (!arr || !path || !order) {
            return null;
        }

        return arr.sort((a: LogEntry, b: LogEntry) => {
          path.forEach(property => {
              a = a[property];
              b = b[property];
          })
          return a > b ? order : order * (-1);
        });
    }
}
