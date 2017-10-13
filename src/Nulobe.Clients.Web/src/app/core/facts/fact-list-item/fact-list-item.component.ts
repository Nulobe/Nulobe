import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Fact, SourceType } from '../../api';

import { Source } from '../../sources';

import { FactLinkResolver } from '../fact-list/fact-list.component';

@Component({
  selector: 'core-fact-list-item',
  templateUrl: './fact-list-item.component.html',
  styleUrls: ['./fact-list-item.component.scss']
})
export class FactListItemComponent implements OnInit {
  @Input() fact: Fact;
  @Input() factLinkResolver: FactLinkResolver;
  @Output() onTagClick = new EventEmitter<string>();

  citedSources: Source[];
  citedDefinitionMarkdown: string;
  notes: string[];

  constructor() { }

  ngOnInit() {
    let sources = (<Source[]>this.fact.sources);
    let sourcesWithIndex = sources.map((source, index) => ({ source, index }));
    let citedSourcesWithIndex = sourcesWithIndex.filter(x => x.source.type !== SourceType.CitationNeeded);

    this.citedSources = sources.filter(s => s.type !== SourceType.CitationNeeded);

    let sourceReferenceRegex = /\[(\d+)\]/g;
    this.citedDefinitionMarkdown = this.fact.definitionMarkdown.replace(sourceReferenceRegex, (match, group) => {
      let sourceNumber = parseInt(group);
      let sourceIndex = sourceNumber - 1;

      let source = sources[sourceIndex];
      let citedSourceIndex = this.citedSources.findIndex(s => s === source); 
      return citedSourceIndex > -1 ? `[${citedSourceIndex + 1}]` : '[Citation needed]';
    });

    this.notes = [this.fact.notesMarkdown, this.fact.creditMarkdown].filter(n => n);
  }

  tagClicked(tag: string) {
    this.onTagClick.emit(tag);
  }

  getUrl(url: string) {
    let parser = document.createElement('a');
    parser.href = url;
    return parser.protocol + '//' + parser.host;
  }

  getNotesSymbolIterator(index: number) {
    return new Array(index + 1);
  }

}